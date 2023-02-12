# Run with Docker

#### To build an image from current dockerfile use this command:

> docker build -t [docker hub id]/platformservice

#### To run built image use:

> docker run -p 8080:80 -d [docker hub id]/platformservice



## K8S

#### To apply k8s deployments, go to the K8S folder and run this:

> kubectl apply -f platform-depl.yaml

#### To apply k8s services, go to the K8S folder and run this:

> kubectl apply -f platforms-np-srv.yaml

#### To refresh deployment in order to k8s pull the latest image from the dockerhub use this command:

> kubectl rollout restart deployment [the id or the name of deployment]