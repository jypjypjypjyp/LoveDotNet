using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using LoveDotNet.Models;
using Microsoft.AspNetCore.Components;

namespace LoveDotNet
{
    public class BlogState
    {
        public event EventHandler StateChanged;

        private readonly HttpClient http;
        public BlogState(HttpClient http)
        {
            this.http = http;
        }

        public async Task<List<Article>> UpdatePage(int curPage)
        {
            return await http.GetJsonAsync<List<Article>>("api/Articles/Page/" + curPage);
        }
        public async Task<List<Article>> UpdatePage(int curPage, int id)
        {
            return await http.GetJsonAsync<List<Article>>("api/Articles/User/"+id+"/Page/" + curPage);
        }

        public async Task<int> GetTotalAmount()
        {
            return await http.GetJsonAsync<int>("api/Articles/TotalAmount");
        }
        public async Task<int> GetTotalAmountByUser(int id)
        {
            return await http.GetJsonAsync<int>("api/Articles/TotalAmount/"+id);
        }

        public async Task<Article> GetArticle(int id)
        {
            return await http.GetJsonAsync<Article>("api/Articles/" + id);
        }

        public async Task<List<Paragraph>> GetParagraphsInArticle(int id)
        {
            return await http.GetJsonAsync<List<Paragraph>>("api/Paragraphs/InArticle/" + id);
        }

        public async Task<List<Comment>> GetCommentsInArticle(int id, int page)
        {
            return await http.GetJsonAsync<List<Comment>>("api/Comments/InArticle/" + id + "/" + page);
        }

        public async Task<int> GetCommentsTotalAmount(int id)
        {
            return await http.GetJsonAsync<int>("api/Comments/TotalAmount/" + id);
        }

        public async Task PostComment(Comment comment)
        {
            bool ok = await http.PostJsonAsync<bool>("api/Comments", comment);
            if (ok) StateHasChanged();
        }

        public async Task<Article> PostArticle(Article article)
        {
            return await http.PostJsonAsync<Article>("api/Articles", article);
        }

        public async Task<Paragraph> PostParagraph(Paragraph paragraph)
        {
            return await http.PostJsonAsync<Paragraph>("api/Paragraphs", paragraph);
        }

        private void StateHasChanged()
        {
            StateChanged?.Invoke(this, EventArgs.Empty);
        }
    }
}
