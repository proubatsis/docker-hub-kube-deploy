# Docker Hub Kubernetes Deployer

This is an ASP.NET Core Web API project. It consumes [Docker Hub Webhooks](https://docs.docker.com/docker-hub/webhooks/)
to listen for updated images and then deploys new images to a Kubernetes cluster.

## Setup

Use helm to install the provided chart. Add as many mappings as needed.

```
helm install --name my-release-name --namespace my-namespace \
--set secret.mapping[0].image="docker-hub-repo/image-name" \
--set secret.mapping[0].deployment="kubernetes-deployment-to-updated" \
--set secret.mapping[1].image="docker-hub-repo/other-image" \
--set secret.mapping[1].deployment="other-deployment" \
./helm-chart
```

A node port service is created, use it for the docker hub webhook url. This service will not deploy images tagged `latest`, the best way to use this project for automatic deployments is to create a `post_push` hook for Docker Hub that tags the image with a unique tag (such as the commit sha) and pushes it to docker hub.
