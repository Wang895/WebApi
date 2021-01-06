using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper.Configuration.Conventions;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.EntityFrameworkCore;

namespace Routine.Helper
{
    public class PagedList<T>:List<T>
    {
        public int PageCount { get; private set; }
        public int CurrentPage { get; private set; }
        public int ItemCount { get; private set; }
        public int PageSize { get; private set; }
        public bool HasPrevious { get; private set; }
        public bool HasNext { get; private set; }
        public PagedList(List<T> list, int count,int pageNumber, int pageSize)
        {
            ItemCount = count;
            CurrentPage = pageNumber;
            PageSize = pageSize;
            PageCount = (int)(Math.Ceiling(ItemCount / (float)PageSize));
            HasPrevious = (CurrentPage > 1);
            HasNext = (CurrentPage < PageCount);
            AddRange(list);
        }

        public static async Task<PagedList<T>> CreateAsync(IQueryable<T> querry,int count, int pageNumber, int pageSize)
        {

            var itemCount=count;
            var item = await querry.Skip(pageSize * (pageNumber - 1)).Take(pageSize).ToListAsync();
            return new PagedList<T>(item, count, pageNumber, pageSize);
        }
    }
}
