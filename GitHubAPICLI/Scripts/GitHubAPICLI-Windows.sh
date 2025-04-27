#!/bin/bash

# Configuraton
GitHubPAT=""
Secret=""
DefaultImage=""
DockerImage="ghcr.io/nano-dna-studios/githubapicli-server:latest"
OutputPort=8080

# Run the Container
docker run --name githubapicli-webhookserver -e GitHubPAT=$GitHubPAT -e Secret=$Secret -e DefaultImage=$DefaultImage --privileged -e DOCKER_HOST=tcp://host.docker.internal:2375 -p $OutputPort:8080 $DockerImage