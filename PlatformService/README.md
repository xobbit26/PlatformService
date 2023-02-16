# Run with Docker

#### To build an image from current dockerfile use this command:

> docker build -t [docker hub id]/platformservice

#### To run built image use:

> docker run -p 8080:80 -d [docker hub id]/platformservice

## K8S

#### To apply k8s deployments, go to the K8S folder and run this:

> kubectl apply -f platforms-depl.yaml

#### To apply k8s services, go to the K8S folder and run this:

> kubectl apply -f platforms-np-srv.yaml

#### To refresh deployment in order to k8s pull the latest image from the dockerhub use this command:

> kubectl rollout restart deployment [the id or the name of deployment]

#### To run the nginx ingress run this command:

> kubectl apply
> -f https://raw.githubusercontent.com/kubernetes/ingress-nginx/controller-v1.5.1/deploy/static/provider/aws/deploy.yaml

IMPORTANT: At first it's better to check the latest version of nginx ingress, because current link might be expired 