#!/bin/bash

# Configuraton
GitHubPAT=""
Secret=""
DefaultImage=""
DockerImage="ghcr.io/nano-dna-studios/githubapicli-server:latest"
OutputPort=8080

# Run the Container
docker run --name githubapicli-webhookserver -e GitHubPAT=$GitHubPAT -e Secret=$Secret -e DefaultImage=$DefaultImage --privileged -v /var/run/docker.sock:/var/run/docker.sock -p $OutputPort:8080 $DockerImage