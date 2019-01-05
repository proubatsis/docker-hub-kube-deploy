# Docker Hub Kubernetes Deployer

This is an ASP.NET Core Web API project. It consumes [Docker Hub Webhooks](https://docs.docker.com/docker-hub/webhooks/)
to listen for updated images and then deploys new images to a Kubernetes cluster.

## Setup

Create a file called `helm-chart/config.json` and add/change values as appropriate:

```
{
  "ImageToDeploymentMapping": {
    "docker-hub-repo/image-name": "kubernetes-deployment-to-update",
    "docker-hub-repo/other-image": "other-deployment"
  },
  "KubernetesNamespace": "my-namespace"
}
```

Use helm to install the provided chart.

```
helm install helm install --name my-release-name --namespace my-namespace ./helm-chart
```

A node port service is created, use it for the docker hub webhook url. This service will not deploy images tagged `latest`, the best way to use this project for automatic deployments is to create a `post_push` hook for Docker Hub that tags the image with a unique tag (such as the commit sha) and pushes it to docker hub.
