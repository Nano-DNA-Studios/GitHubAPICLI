#FROM ubuntu:22.04
#
## Install Docker properly
#RUN curl -fsSL https://download.docker.com/linux/ubuntu/gpg | sudo apt-key add - && \
#    add-apt-repository "deb [arch=amd64] https://download.docker.com/linux/ubuntu focal stable" && \
#    apt-get update && \
#    apt-get install -y docker-ce docker-ce-cli containerd.io
#
#FROM mcr.microsoft.com/dotnet/aspnet:8.0

FROM ubuntu:22.04

# Install docker
RUN apt-get update && apt-get install -y \
    apt-transport-https \
    ca-certificates \
    curl \
    software-properties-common && \
    curl -fsSL https://download.docker.com/linux/ubuntu/gpg | apt-key add - && \
    add-apt-repository "deb [arch=amd64] https://download.docker.com/linux/ubuntu focal stable" && \
    apt-get update && \
    apt-get install -y docker-ce docker-ce-cli containerd.io

# Install .NET Runtime
RUN apt-get update && \
    apt-get install -y wget && \
    wget https://packages.microsoft.com/config/ubuntu/22.04/packages-microsoft-prod.deb -O packages-microsoft-prod.deb && \
    dpkg -i packages-microsoft-prod.deb && \
    apt-get update && \
    apt-get install -y aspnetcore-runtime-8.0

# Copy your published app
COPY GitHubAPICLI/bin/Release/net8.0/linux-x64/publish /GitHubAPICLI

# Make sure OutputLogs exists
RUN mkdir /GitHubAPICLI/OutputLogs

EXPOSE 8080

# Final command to run
CMD ["/bin/bash", "-c", "\
/GitHubAPICLI/GitHubAPICLI registerpat \"$GitHubPAT\" && \
/GitHubAPICLI/GitHubAPICLI getrepository Nano-DNA-Studios GitHubAPICLI && \
/GitHubAPICLI/GitHubAPICLI registerwebhookserver \"$Secret\" \"$DefaultImage\" 8080 /GitHubAPICLI/OutputLogs && \
/GitHubAPICLI/GitHubAPICLI startwebhookserver && \
/bin/bash"]
