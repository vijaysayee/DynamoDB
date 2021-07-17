using Docker.DotNet;
using Docker.DotNet.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;

namespace MovieRank.Integration.Tests.Setup
{
    public class TestContext : IAsyncLifetime
    {
        private const string ContainerImageUri = "amazon/dynamodb-local";
        private readonly DockerClient dockerClient;
        private string containerId;

        public TestContext()
        {
            this.dockerClient = new DockerClientConfiguration(new Uri("npipe://./pipe/docker_engine")).CreateClient();
        }

        public async Task DisposeAsync()
        {
            if (containerId != null)
            {
                await this.dockerClient.Containers.KillContainerAsync(containerId, new ContainerKillParameters());
            }
        }

        public async Task InitializeAsync()
        {
            await PullImage();

            await StartContainer();

            await new TestDataSetup().CreateTable();
        }

        private async Task PullImage()
        {
            await dockerClient.Images.CreateImageAsync(new ImagesCreateParameters
            {
                FromImage = ContainerImageUri,
                Tag = "latest"
            },
            new AuthConfig(),
            new Progress<JSONMessage>());

        }

        private async Task StartContainer()
        {
            var response = await dockerClient.Containers.CreateContainerAsync(new CreateContainerParameters
            {
                Image = ContainerImageUri,
                ExposedPorts = new Dictionary<string, EmptyStruct>
                {
                    { 
                        "8000", default(EmptyStruct)
                    }
                },
                HostConfig = new HostConfig
                { 
                    PortBindings = new Dictionary<string, IList<PortBinding>>
                    {
                        { "8000", new List<PortBinding> { new PortBinding { HostPort = "8000" } } }
                    },
                    PublishAllPorts = true
                }

            });

            this.containerId = response.ID;

            await this.dockerClient.Containers.StartContainerAsync(containerId, null);
        }
    }
}