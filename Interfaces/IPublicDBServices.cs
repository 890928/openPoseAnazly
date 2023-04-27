using System;
using Webapi.Models;
using System.Threading.Tasks;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using Webapi.services;

namespace Webapi.interfaces
{
    public interface IPublicDBServices
    {

        /// <summary>
        /// 產生截圖次數
        /// </summary>
        /// <param name="DetectionId">檢測Id</param>
        Task<int> Getcount(string DetectionId);
    }
}