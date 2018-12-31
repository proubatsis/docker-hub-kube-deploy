using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using DeployApi.Models.DockerHub;
using System.Net;
using DeployApi.Services;

namespace DeployApi.Controllers
{
    [Route("api/docker-hub")]
    [ApiController]
    public class DockerHubController : ControllerBase
    {
        private const string LATEST_TAG = "latest";

        private IKubernetesApiService _service;

        // POST api/values
        [HttpPost]
        public ActionResult<WebhookCallbackModel> Post([FromBody] WebhookRequestModel webhook)
        {
            if (webhook.PushData.Tag == LATEST_TAG) {
                return Accepted(new WebhookCallbackModel {
                    State = "success",
                    Description = "Image not deployed because tag is \"latest\""
                });
            }

            return new WebhookCallbackModel {
                State = "success",
                Description = "Image deployed",
                Context = webhook.CallbackUrl
            };
        }
    }
}
