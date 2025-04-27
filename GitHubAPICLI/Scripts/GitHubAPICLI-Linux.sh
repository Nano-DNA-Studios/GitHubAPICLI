#!/bin/bash

# Configuraton
GitHubPAT=""
Secret=""
DefaultImage=""

# Run the Container
docker run --name githubapicli-webhookserver -e GitHubPAT=$GitHubPAT -e Secret=$Secret -e DefaultImage=$DefaultImage --privileged -v /var/run/docker.sock:/var/run/docker.sock -p 8080:8080 dotnet-idk