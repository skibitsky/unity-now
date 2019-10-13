using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.Security.Cryptography;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace com.skibitsky.UnityNow
{
    public static partial class DeployNow
    {
        [MenuItem("Now/Deploy", false, 0)]
        public static async void Deploy()
        {

            var configuration = AssetDatabase.LoadAssetAtPath<ConfigureNow>("Assets/unity-now/ConfigureNow.asset");

            // Check if access token is not empty
            if (string.IsNullOrWhiteSpace(configuration.Token))
            {
                Debug.LogError("[unity-now]: Zei Now access token is missing. Please set token in Assets/unity-now/ConfigureNow." +
                               "\nYou can create a new token at: <i>https://zeit.co/account/tokens</i>");
                return;
            }
            
            // Use Unity project name for deployment name
            var deploymentName = PlayerSettings.productName;
            
            var httpClient = new HttpClient();
            
            var deploymentFileReferences = new ConcurrentBag<DeploymentFileReference>();
            var uploadFileRequests = new ConcurrentBag<Task>();
            
            var path = EditorUtility.OpenFolderPanel("Select the folder to deploy", "", "");
            
            Parallel.ForEach(Directory.EnumerateFiles(path, "*", SearchOption.AllDirectories), file =>
            {
                var deploymentFile = new DeploymentFile();

                using (var stream = File.OpenRead(file))
                {
                    // File size in bytes
                    var contentLength = stream.Length;
                    deploymentFile.FileSize = contentLength;

                    // File relative path
                    deploymentFile.RelativePath = file.Replace($"{path}/", "");
                    
                    // File data
                    var data = new byte[contentLength];
                    stream.Read(data, 0, (int)contentLength);
                    deploymentFile.Data = data;
                    
                    // File SHA1 used to check the integrity
                    using (var sha = SHA1.Create())
                        deploymentFile.Checksum = BitConverter.ToString(sha.ComputeHash(data)).ToLowerInvariant()
                            .Replace("-", "");
                }

                deploymentFileReferences.Add(new DeploymentFileReference(deploymentFile));
                uploadFileRequests.Add(UploadFileRequest(deploymentFile, httpClient, configuration));
            });

            // Wait for all files to upload
            await Task.WhenAll(uploadFileRequests);
            Debug.Log($"[unity-now]: All files synced! Creating deployment for <i>{deploymentName}</i>...");

            // Create Deployment
            await CreateDeploymentRequest(deploymentFileReferences, deploymentName, httpClient, configuration);
        }

        /// <summary>
        /// Creates new Now deployment
        /// </summary>
        /// <see cref="https://zeit.co/docs/api/#endpoints/deployments/create-a-new-deployment"/>
        private static async Task CreateDeploymentRequest(IEnumerable<DeploymentFileReference> deploymentFileReferences,
            string deploymentName, HttpMessageInvoker httpClient, ConfigureNow configuration, CancellationToken cancellationToken = default)
        {
            var content = new DeploymentContent()
            {
                Name = deploymentName,
                Version = 2,
                Files = deploymentFileReferences.ToArray()
            };
            
            var jsonContent = JsonConvert.SerializeObject(content);

            var httpMessage = new HttpRequestMessage
            {
                Method = HttpMethod.Post,
                RequestUri = new Uri($"{configuration.BaseUrl}/deployments"),
                Headers =
                {
                    {"Authorization", $"Bearer {configuration.Token}"}
                },
                Content = new StringContent(jsonContent, Encoding.UTF8, "application/json"),
            };

            using (var response = await httpClient.SendAsync(httpMessage, cancellationToken))
            {
                var responseMessage = await response.Content.ReadAsStringAsync();
                if (response.IsSuccessStatusCode)
                {
                    var jObject = JObject.Parse(responseMessage);
                    var deploymentUrl = jObject["url"].ToString();
                    
                    // Copy URL to clipboard
                    if(configuration.CopyUrl)
                        GUIUtility.systemCopyBuffer = deploymentUrl;
                    
                    Debug.Log($"[unity-now]: Ready! <i>https://{deploymentUrl}</i> " +
                              $"{(configuration.CopyUrl ? "(copied)" : "" )}");
                }
                else
                {
                    Debug.LogError($"[unity-now]: Something went wrong during deployment creation.\n{responseMessage}");
                }
            }
                
        }
        
        /// <summary>
        /// Uploads one file to the Now
        /// </summary>
        /// <see cref="https://zeit.co/docs/api/#endpoints/deployments/upload-deployment-files"/>
        private static async Task UploadFileRequest(DeploymentFile deploymentFile, HttpMessageInvoker httpClient, ConfigureNow configuration, 
            CancellationToken cancellationToken = default)
        {
            var httpMessage = new HttpRequestMessage
            {
                Method = HttpMethod.Post,
                RequestUri = new Uri($"{configuration.BaseUrl}/files"),
                Headers =
                {
                    {"Authorization", $"Bearer {configuration.Token}"},
                    {"x-now-digest", deploymentFile.Checksum},
                },
                Content = new ByteArrayContent(deploymentFile.Data),
            };
            
            httpMessage.Content.Headers.ContentLength = deploymentFile.FileSize;

            using (var response = await httpClient.SendAsync(httpMessage, cancellationToken))
            {
                var responseMessage = await response.Content.ReadAsStringAsync();
                if (!response.IsSuccessStatusCode)
                    Debug.LogError($"[unity-now]: Something went wrong during file upload.\n{responseMessage}");
            }                
        }
    }
}