using Microsoft.Xrm.Sdk;
using System;
using System.Net;
using System.ServiceModel;

namespace Student7Plugin
{
    public class UpdateResponse : IPlugin

    {

        public void CreateResponse(Guid id, string Response)
        {
            var data = "{InquiryId:" + id.ToString()+",Response:"+Response + "}";
            var client = new WebClient();
            client.Headers.Add(HttpRequestHeader.ContentType, "application/json");
            //for now, use this dummy uri 
            client.UploadStringAsync(new Uri("http://rest.learncode.academy/API/Yichao/inquiries/"+id.ToString()), "PUT", data);
        }

        /// <summary>
        /// A plug-in that creates a follow-up task activity when a new account is created.
        /// </summary>
        /// <remarks>Register this plug-in on the Create message, account entity,
        /// and asynchronous mode.
        /// </remarks>
        /// 
       
        public void Execute(IServiceProvider serviceProvider)
        {
            //Extract the tracing service for use in debugging sandboxed plug-ins.
            ITracingService tracingService =
                (ITracingService)serviceProvider.GetService(typeof(ITracingService));
            // Obtain the execution context from the service provider.
            IPluginExecutionContext context = (IPluginExecutionContext)serviceProvider.GetService(typeof(IPluginExecutionContext));

            // The InputParameters collection contains all the data passed in the message request.
            if (context.InputParameters.Contains("Target") &&
                context.InputParameters["Target"] is Entity)
            {
                // Obtain the target entity from the input parameters.
                Entity entity = (Entity)context.InputParameters["Target"];

                // Verify that the target entity represents an account.
                // If not, this plug-in was not registered correctly.
                if (entity.LogicalName != "new_inquiry")
                    return;
                //using (WebClient client = new WebClient())
                //{
                    try
                    {
                    FaultException ex = new FaultException();
                        var response = entity.GetAttributeValue<string>("new_response");
                        tracingService.Trace("Plugin is working");
                        //throw new InvalidPluginExecutionException("Plugin is working", ex);
                        CreateResponse(entity.Id, response);
                       
                    }
                    catch (FaultException<OrganizationServiceFault> ex)
                    {
                        throw new InvalidPluginExecutionException("An error occurred in the UpdateResponse plug-in.", ex);
                    }
                    catch (Exception ex)
                    {
                        tracingService.Trace("UpdateResponse: {0}", ex.ToString());
                        throw;
                    }
                //}
            }
        }
    }

    public class DeleteQuestion : IPlugin

    {

        public void DestroyQuestion(Guid id)
        {
            var client = new WebClient();
            client.Headers.Add(HttpRequestHeader.ContentType, "application/json");
            //for now, use this dummy uri 
            client.UploadStringAsync(new Uri("http://rest.learncode.academy/API/Yichao/inquiries/" + id.ToString()), "DELETE");
        }

        /// <summary>
        /// A plug-in that creates a follow-up task activity when a new account is created.
        /// </summary>
        /// <remarks>Register this plug-in on the Create message, account entity,
        /// and asynchronous mode.
        /// </remarks>
        /// 

        public void Execute(IServiceProvider serviceProvider)
        {
            //Extract the tracing service for use in debugging sandboxed plug-ins.
            ITracingService tracingService =
                (ITracingService)serviceProvider.GetService(typeof(ITracingService));
            // Obtain the execution context from the service provider.
            IPluginExecutionContext context = (IPluginExecutionContext)serviceProvider.GetService(typeof(IPluginExecutionContext));

            // The InputParameters collection contains all the data passed in the message request.
            if (context.InputParameters.Contains("Target") &&
                context.InputParameters["Target"] is Entity)
            {
                // Obtain the target entity from the input parameters.
                Entity entity = (Entity)context.InputParameters["Target"];

                // Verify that the target entity represents an account.
                // If not, this plug-in was not registered correctly.
                if (entity.LogicalName != "new_inquiry")
                    return;
                //using (WebClient client = new WebClient())
                //{
                try
                {
                    FaultException ex = new FaultException();
                    tracingService.Trace("Plugin is working");
                    //throw new InvalidPluginExecutionException("Plugin is working", ex);
                    DestroyQuestion(entity.Id);

                }
                catch (FaultException<OrganizationServiceFault> ex)
                {
                    throw new InvalidPluginExecutionException("An error occurred in the UpdateResponse plug-in.", ex);
                }
                catch (Exception ex)
                {
                    tracingService.Trace("UpdateResponse: {0}", ex.ToString());
                    throw;
                }
                //}
            }
        }
    }
}
