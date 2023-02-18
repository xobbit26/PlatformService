# Run with Docker

#### To build an image from current dockerfile use this command:
> docker build -t [docker hub id]/platformservice

#### To run built image use:
> docker run -p 8080:80 -d [docker hub id]/platformservice

#### To run pgAdmin image use:
> docker run -p 5050:80 -e "PGADMIN_DEFAULT_EMAIL=admin@admin.com" -e "PGADMIN_DEFAULT_PASSWORD=admin" -d dpage/pgadmin4


# K8S

#### To apply k8s files use this command:
> kubectl apply -f [the name of needed file]

#### To refresh deployment in order to k8s pull the latest image from the dockerhub use this command:
> kubectl rollout restart deployment [the id or the name of deployment]

#### To run the nginx ingress run this command:
> kubectl apply
> -f https://raw.githubusercontent.com/kubernetes/ingress-nginx/controller-v1.5.1/deploy/static/provider/aws/deploy.yaml

#### To create secret FOR MSSQL USE THIS:
>kubectl create secret generic mssql --from-literal=SA_PASSWORD="[your password]"


IMPORTANT: At first it's better to check the latest version of nginx ingress, because current link might be expired

### To get kubectl resources run these commands:
#### Get deployments:
>kubectl get deployments

#### Get services:
>kubectl get services

#### Get pods:
>kubectl get pods
> 
#### Get persistent volume claims:
>kubectl get pvc