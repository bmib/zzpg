﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Solution.Web.Controllers
{
    public class WeightController : BaseController
    {
        //
        // GET: /Weight/
        public ActionResult ItemSortView()
        {
            return View();
        }

        public ActionResult ItemDegreeView()
        {
            return View();
        }
	}
}