using Microsoft.AspNetCore.Components;
using System;
using System.Net.Http;
using System.Threading.Tasks;
using LoveDotNet.Models;
using Newtonsoft.Json;
using LoveDotNet.Helpers;

namespace LoveDotNet
{
    public class UserState
    {
        public event EventHandler UserChanged;
        public event EventHandler UserInfoChanged;
        public User CurrentUser { get; set; } = new User();
        public bool ShowLoginDialog { get; set; }
        public bool ShowUpdateDialog { get; set; }

        private readonly HttpClient http;
        public UserState(HttpClient http)
        {
            this.http = http;
        }

        public async Task<User> GetUser(int id)
        {
            return await http.GetJsonAsync<User>("api/Users/" + id);
        }

        public async Task<bool> UpdateUser(int id, User user)
        {
            var json = JsonConvert.SerializeObject(user);
            var stringContent = new StringContent(json, System.Text.Encoding.UTF8, "application/json");
            var result = await http.PutAsync("api/Users/" + id, stringContent);

            if (result.StatusCode == System.Net.HttpStatusCode.NoContent)
            {
                CurrentUser = user;
                ShowUpdateDialog = false;
                UserInfoHasChanged();
                return true;
            }
            UserInfoHasChanged();
            return false;
        }

        public async Task<bool> Login(string email, string passwd)
        {
            var result = await http.PostJsonAsync<User>("api/Users/Login", new User() { Email = email, Password = passwd });
            if (result.IsEmpty())
                return false;
            CurrentUser = result;
            ShowLoginDialog = false;
            UserHasChanged();
            return false;
        }
        public async Task<bool> Signup(string email, string passwd)
        {
            var result = await http.PostJsonAsync<User>("api/Users/Signup", new User() { Email = email, Password = passwd });
            if (result.IsEmpty())
                return false;
            CurrentUser = result;
            ShowLoginDialog = false;
            UserHasChanged();
            EmailHelper.SendAsyncWithoutAwait(
                CurrentUser.Email,
                "【LoveDotNet】欢迎加入",
                string.Format(
@"<p>{0}：<br>
<p> 注册成功！请存档本邮件以方便找回密码，您的密码为{1}<br>
<p> --LoveDotNet <br>", CurrentUser.Email, CurrentUser.Password));
            return true;
        }

        public void Signout()
        {
            CurrentUser = new User();
            UserHasChanged();
        }

        public async Task<bool> Apply()
        {
            if (CurrentUser.IsEmpty()) return false;
            string code = "Apply" + CurrentUser.Id;
            if (await EmailHelper.SendAsync(
                "1104462345@qq.com",
                "【LoveDotNet】申请成为编辑",
                string.Format(
@"<p>蛤蛤小子：<br>
<p> {0}想成为编辑，代码{1}！<br>
<p> --LoveDotNet <br>", CurrentUser.Email, code)))
            {
                ActionHelper.ActionDict[code] = (CurrentUser.Id, ActionType.Apply);
                return true;
            }
            else
            {
                return false;
            }
        }

        public async Task<bool> MakeEditor(int id)
        {
            var u = await http.GetJsonAsync<User>("api/Users/" + id);
            u.IsEditor = true;
            var json = JsonConvert.SerializeObject(u);
            var stringContent = new StringContent(json, System.Text.Encoding.UTF8, "application/json");
            var result = await http.PutAsync("api/Users/" + id, stringContent);

            if (result.StatusCode == System.Net.HttpStatusCode.NoContent)
            {
                await EmailHelper.SendAsync(
                u.Email,
                "【LoveDotNet】您已成为本站编辑",
                string.Format(
@"<p>{0}：<br>
<p> 您已成为本站编辑！<br>
<p> --LoveDotNet <br>", u.Email));
                return true;
            }
            else
            {
                return false;
            }
        }

        private void UserHasChanged()
        {
            UserChanged?.Invoke(this, EventArgs.Empty);
        }
        private void UserInfoHasChanged()
        {
            UserInfoChanged?.Invoke(this, EventArgs.Empty);
        }
    }
}
