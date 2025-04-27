#!/bin/bash

# Configuraton
GitHubPAT=""
Secret=""
DefaultImage=""

# Run the Container
docker run --name githubapicli-webhookserver -e GitHubPAT=$GitHubPAT -e Secret=$Secret -e DefaultImage=$DefaultImage --privileged -e DOCKER_HOST=tcp://host.docker.internal:2375 -p 8080:8080 dotnet-idk