using System;
using Webapi.Models;
using System.Threading.Tasks;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Webapi.interfaces;
using Webapi.InversionOfControl;

namespace Webapi.services
{
    [RegisterIOC]
    public class PublicDBServices : IPublicDBServices
    {
        public readonly OpenposeContext _openposeContext;
        public PublicDBServices(OpenposeContext openposeContext
        )
        {
            _openposeContext = openposeContext;
        }

        /// <summary>
        /// 產生截圖次數
        /// </summary>
        /// <param name="DetectionId">檢測Id</param>
        /// <returns>截圖次數</returns>
        public async Task<int> Getcount(string DetectionId)
        {
            var count = await _openposeContext.Result
                             .CountAsync(x => x.DetectionId == DetectionId);
            return count;
        }
    }
}