﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SoftEng2BackendAPI.Repositories
{
    public interface IUserRepository
    {
        IEnumerable<UserModel> FetchUsers();
        UserModel FetchSpecificUser(int user_id);

        bool LoginUser(string username, string password);

    }
}