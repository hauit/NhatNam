using DevExtreme.AspNet.Data;
using DevExtreme.AspNet.Mvc;
using NNworking.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace NNworking.Controllers.ProcessingDept
{
    [Route("api/Phong-san-xuat/{action}", Name = "ProcessingAPI")]
    public class ProcessingAPIControllerController : ApiController
    {
        private NN_DatabaseEntities db = new NN_DatabaseEntities();
    }
}
