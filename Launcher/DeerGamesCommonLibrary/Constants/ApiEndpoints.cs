using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DeerGamesCommonLibrary.Constants
{
    public static class ApiEndpoints
    {
        public const string Login = "users/login"; // POST
        public const string SignUp = "users/sign-up"; // POST
        public const string ConnectionTest = "ranking/get"; // GET

        // Flyer
        public const string FlyerRankingAll = "ranking/get";
        public const string FlyerRankingPrivate = "ranking/get/personal";
        public const string FlyerRankingSpecific = "ranking/get?name=";
        public const string FlyerRankingCreate = "ranking/create";
    }
}
