﻿using System.Threading.Tasks;

namespace OmsApi.Interfaces
{
    public interface ISmsSender
    {
        Task SendSmsAsync(string number, string message);
    }
}