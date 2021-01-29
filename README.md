<div align="center">
<img src=".github/logo.png" width="394" align="center"></br>
<h1 align="center">Unity Now</h1>
<p align="center">
Deploy Unity WebGL builds on Vercel serverless platform with ease.
</p>
<a href="https://openupm.com/packages/com.skibitsky.UnityNow/"><img src="https://img.shields.io/npm/v/com.skibitsky.UnityNow?label=openupm&amp;registry_uri=https://package.openupm.com" /></a>
</div>

## Installation

### Install via OpenUPM

The package is available on the [openupm registry](https://openupm.com). It's recommended to install it via [openupm-cli](https://github.com/openupm/openupm-cli).

```
openupm add com.skibitsky.unity-now
```

### Install via Git URL

Open *Packages/manifest.json* with your favorite text editor. Add the following line to the dependencies block.

    {
        "dependencies": {
            "com.skibitsky.unity-now": "https://github.com/skibitsky/unity-now.git"
        }
    }

Notice: Unity Package Manager records the current commit to a lock entry of the *manifest.json*. To update to the latest version, change the hash value manually or remove the lock entry to resolve the package.

    "lock": {
      "com.skibitsky.unity-now": {
        "revision": "master",
        "hash": "..."
      }
    }


## Usage
0. Add your [access token](https://vercel.com/account/tokens) to the **Configure Now** assets (Assets/ConfigureNow)
1. Run Now→ Deploy from the menu bar
	<br><img src=".github/screenshot1.png" width="300">
2. Select your WebGL build
3. Wait till deployment completes

## Configuration
You can configure Unity Now using **Configure Now** scriptable object. By default it is located at *Assets/ConfigureNow* and contains the following properties:

| Name | Description |
| --- | --- |
| **Token** | Zeit Now access token. You can generate a new one [here](https://vercel.com/account/tokens) |
| **Base URL** | Endpoint base URL. You can change it if you need a certain server location. [Read more](https://zeit.co/docs/api/#api-basics/server-specs/origins) |
| **Copy URL** | If enabled, Unity Now will save the deployment URL to the clipboard after the deployment is complete |

## License
MIT © [skibitsky](http://skibitsky.com)
