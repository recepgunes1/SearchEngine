#!/bin/bash

if [ $# -ne 2 ]; then
    echo "Usage: $0 [up|down] [production|development]"
    exit 1
fi

action=$1
environment=$2

if [ "$action" != "up" ] && [ "$action" != "down" ]; then
    echo "First argument must be 'up' or 'down'"
    exit 1
fi

if [ "$environment" != "production" ] && [ "$environment" != "development" ]; then
    echo "Second argument must be 'production' or 'development'"
    exit 1
fi

echo "Bringing $2 environment $1..."

if [ "$action" == "up" ]; then
    if [ "$environment" == "production" ]; then
        docker compose -f docker-compose.yml -f ./deployments/Development/docker-compose.development.yml --env-file ./deployments/Production/.env.Production build
        kubectl apply -f ./deployments/Production/namespaces.yaml
        for file in `find ./deployments/Production/ -name "*.yaml" -not -name "namespaces.yaml"`
        do
            kubectl apply -f $file
        done
    else
        docker compose -f docker-compose.yml -f ./deployments/Development/docker-compose.development.yml --env-file ./deployments/Development/.env.Development up -d
    fi
else
    if [ "$environment" == "production" ]; then
        kubectl delete deployment --all --namespace=services
        kubectl delete deployment --all --namespace=databases
        kubectl delete deployment --all --namespace=rabbitmq

        kubectl delete service --all --namespace=services
        kubectl delete service --all --namespace=databases
        kubectl delete service --all --namespace=rabbitmq

        kubectl delete namespace services
        kubectl delete namespace databases
        kubectl delete namespace rabbitmq
    else
        docker compose -f docker-compose.yml -f ./deployments/Development/docker-compose.development.yml --env-file ./deployments/Development/.env.Development down --volumes  
    fi
fi

# docker rmi $(docker images --format "{{.Repository}}:{{.ID}}" | grep -e searchengine -e "<none>" | cut -d : -f 2)
