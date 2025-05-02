# GitHubAPICLI
---
A CLI Tool for Communication with the ``GitHub API`` for ``GitHub Actions`` related tasks. GitHub Action Workflow Configurations can be setup to Spawn and Fill in Workflows using ``Docker Containers`` as needed. CLI Tool has a ``Webhook Server`` built in for Automatic Action Worker Dispatching and Cleanup, this allows you to Setup Ephemeral Workflows.

# About
----
This CLI Tool was developed in order to create Ephemeral, Self Hosted GitHub Action Runners for Automated CI/CD Build Pipelines. 

After learning about GitHub Action Workers on the Job and utilizing a Self Hosted Instance on a Server and Running into a Building Bottle neck a Coworker approached me with the Idea of Ephemeral GitHub Action Workers, that replicate the Cloud GitHub Action Workers that start on the fly when Workflows are Requested. 

Alongside this tool, some base Libraries were developed in Isolation to allow users to develop their own versions of this tool or implement chunks into unrelated projects. These Libraries each have their own NuGet Packages published that can be used freely which are listed here.

## Requirements
----
- .NET 8 or Later installed
- GitHub PAT (Personal Access Token)
	- API Communication Scope : ``repo``
	- Ephemeral Runners Scope : ``workflow``
- Docker is Installed (For GitHub Action Runners)

## Installation
----
There are multiple ways of installing this tool and using it. Common methods are to Download the Self-Contained Builds, Install it from NuGet or Cloning it from GitHub.

### Download Self Contained Build
----
Go to the [``Release``](https://github.com/Nano-DNA-Studios/GitHubAPICLI/releases) Page of the Repository and Download the Tools Version with the Features you want for your Target Platform and OS.

### Install from NuGet
----
Use the following command to install the Tool. Replace ``<version>`` with the appropriate version using ``0.0.0`` format.

```bash
dotnet tool install --global GitHubAPICLI --version <version>
```

### Clone and Build
---
Clone the latest state of the Repo and Build it locally.

```bash
git clone https://github.com/Nano-DNA-Studios/GitHubAPICLI
cd GitHubAPICLI
dotnet build
```

## Library Dependencies
---
The GitHub API CLI Application relies on the Following NuGet Packages produced inhouse, these libraries can be freely used in accordance with the MIT License.

Libraries :
- [NanoDNA.GitHubManager](https://github.com/Nano-DNA-Studios/NanoDNA.GitHubManager) - Manages GitHub API Communication and provides a Class to Control GitHub Action Runners
- [NanoDNA.DockerManager](https://github.com/Nano-DNA-Studios/NanoDNA.DockerManager) - Manages Docker Communication and usage. Can Create and Control Containers from C#
- [NanoDNA.ProcessRunner](https://github.com/Nano-DNA-Studios/NanoDNA.ProcessRunner) - Manages Background Processes. Allows for System / Shell / Process Dispatches and receiving results.

# License
----
Individuals can use the Tool under the MIT License

Groups and or Companies consisting of 5 or more people can Contact @MrDNAlex to License the Tool for usage. 

## Support
----
For Additional Support for GitHubAPICLI you can Contact @MrDNAlex through the email : ``Mr.DNAlex.2003@gmail.com``.