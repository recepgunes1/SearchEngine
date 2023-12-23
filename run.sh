#!/bin/bash

if [ $# -ne 2 ]; then
    echo "Usage: $0 [up|down] [production|development|staging|preproduction]"
    exit 1
fi

action=$1
environment=$2

if [ "$action" != "up" ] && [ "$action" != "down" ]; then
    echo "First argument must be 'up' or 'down'"
    exit 1
fi

if [ "$environment" != "production" ] && [ "$environment" != "development" ] && [ "$environment" != "staging" ] && [ "$environment" != "preproduction" ]; then
    echo "Second argument must be 'production' or 'development' or 'staging' or 'preproduction'"
    exit 1
fi

echo "Bringing $2 environment $1..."

if [ "$action" == "up" ]; then
    if [ "$environment" == "production" ]; then
        docker compose -f docker-compose.yml -f ./deployments/Staging/docker-compose.staging.yml --env-file ./deployments/Production/.env.Production build
        kubectl apply -f ./deployments/Production/namespaces.yaml
        for file in `find ./deployments/Production/ -name "*.yaml" -not -name "namespaces.yaml"`
        do
            kubectl apply -f $file
        done
    elif [ "$environment" == "preproduction" ]; then
        docker compose -f docker-compose.yml -f ./deployments/PreProduction/docker-compose.preproduction.yml --env-file ./deployments/PreProduction/.env.PreProduction up -d
    elif [ "$environment" == "staging" ]; then
        docker compose -f docker-compose.yml -f ./deployments/Staging/docker-compose.staging.yml --env-file ./deployments/Staging/.env.staging up -d
    else
        docker compose -f ./deployments/Development/docker-compose.development.yml --env-file ./deployments/Development/.env.Development up -d
        echo "External services are running right now..."
    fi
else
    if [ "$environment" == "production" ]; then
        for ns in services databases rabbitmq clients elastic; do
            kubectl delete deployment --all --namespace=$ns
            kubectl delete service --all --namespace=$ns
            kubectl delete namespace $ns
        done
        docker rmi $(docker images --format "{{.Repository}}:{{.ID}}" | grep -e searchengine -e "<none>" | cut -d : -f 2)
    elif [ "$environment" == "preproduction" ]; then
        docker compose -f docker-compose.yml -f ./deployments/PreProduction/docker-compose.staging.yml --env-file ./deployments/PreProduction/.env.staging down
        docker rmi $(docker images --format "{{.Repository}}:{{.ID}}" | grep -e searchengine -e "<none>" | cut -d : -f 2)
    elif [ "$environment" == "staging" ]; then
        docker compose -f docker-compose.yml -f ./deployments/Staging/docker-compose.staging.yml --env-file ./deployments/Staging/.env.staging down
        docker rmi $(docker images --format "{{.Repository}}:{{.ID}}" | grep -e searchengine -e "<none>" | cut -d : -f 2)
    else
        docker compose -f ./deployments/Development/docker-compose.development.yml --env-file ./deployments/Development/.env.Development down
        echo "External services are disposed..."
    fi
fi

# docker rmi $(docker images --format "{{.Repository}}:{{.ID}}" | grep -e searchengine -e "<none>" | cut -d : -f 2)
