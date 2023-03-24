DOCKER_IMAGE=imagerecorder_app
# Use INTERNAL port for the communication inside the docker network (1433 not 11433)# Cleanup
docker rm -f $DOCKER_IMAGE
docker volume prune -f
docker image prune -f
# Run SQL Server container with assigned ip in docker network.
docker run