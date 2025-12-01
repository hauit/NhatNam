using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace NNworking.Controllers
{
    public class WorkFolowController : BaseController
    {
        // GET: WorkFolow
        public ActionResult Index()
        {
            return View();
        }

        /// <summary>
        /// Work follow definition
        /// </summary>
        /// <returns></returns>
        public ActionResult WorkflowDefinition()
        {
            return View();
        }

        /// <summary>
        /// Work flow step, it is child of work flow definition
        /// </summary>
        /// <returns></returns>
        public ActionResult WorkflowStep()
        {
            return View();
        }

        /// <summary>
        /// Work follow action type, it will be used in work flow step
        /// </summary>
        /// <returns></returns>
        public ActionResult WorkFolowActionType()
        {
            return View();
        }

        /// <summary>
        /// Work follow role, it will be used in work flow step
        /// </summary>
        /// <returns></returns>
        public ActionResult WorkFolowRole()
        {
            return View();
        }

        /// <summary>
        /// Work follow instance, it wrap WorkFolow definition and its steps to use in real case(using on WorkFolowModule)
        /// </summary>
        /// <returns></returns>
        public ActionResult WorkFolowInstance()
        {
            return View();
        }

        /// <summary>
        /// Work follow instance history, it is history of work follow instance
        /// </summary>
        /// <returns></returns>
        public ActionResult WorkFolowInstanceHistory()
        {
            return View();
        }

        /// <summary>
        /// Work follow module instance history, it is work follow instance on each module
        /// </summary>
        /// <returns></returns>
        public ActionResult WorkFolowModuleDefinition()
        {
            return View();
        }
    }
}