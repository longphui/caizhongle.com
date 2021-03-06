﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Lottery.Entity;
using Webdiyer.WebControls.Mvc;

namespace Lottery.Web.Controllers
{
    public class AdminController : Controller
    {
        //登录页面
        public ActionResult Index()
        {
            return View();
        }

        //后台首页
        public ActionResult Home()
        {
            return View();
        }

        //用户列表
        public ActionResult UserList(UserInfo userInfo, PageInfo pageInfo)
        {
            if (pageInfo == null) pageInfo = new PageInfo();
            IList<UserInfo> userInfos = Lottery.DatabaseProvider.Instance().GetUser(userInfo, null).ToPagedList(pageInfo.PageIndex.Value, pageInfo.PageSize.Value);
            return View(userInfos);
        }

        //视频列表
        public ActionResult VideoList(VideoInfo videoInfo, PageInfo pageInfo)
        {
            if (pageInfo == null) pageInfo = new PageInfo();
            IList<VideoInfo> videoInfos = Lottery.DatabaseProvider.Instance().GetVideo(videoInfo, null).ToPagedList(pageInfo.PageIndex.Value, pageInfo.PageSize.Value);
            IList<VideoCategoryInfo> videoCategoryInfos = Lottery.DatabaseProvider.Instance().GetVideoCategory(null);
            ViewBag.VideoCategoryInfos = ViewCategoryTree("0", videoCategoryInfos, null);
            return View(videoInfos);
        }

        //视频分类列表
        public ActionResult VideoCategoryList(VideoCategoryInfo videoCategoryInfo, PageInfo pageInfo)
        {
            if (pageInfo == null) pageInfo = new PageInfo();
            IList<VideoCategoryInfo> videoCategoryInfos = Lottery.DatabaseProvider.Instance().GetVideoCategory(videoCategoryInfo);
            videoCategoryInfos = ViewCategoryTree("0", videoCategoryInfos,null);
            return View(videoCategoryInfos);
        }

        private IList<VideoCategoryInfo> ViewCategoryTree(string PID, IList<VideoCategoryInfo> videoCategoryInfos, IList<VideoCategoryInfo> resultVideoCategoryInfo)
        {
            if (resultVideoCategoryInfo == null)
            {
                resultVideoCategoryInfo = new List<VideoCategoryInfo>();
            }
            if (videoCategoryInfos != null)
            {
                foreach (VideoCategoryInfo item in videoCategoryInfos)
                {
                    if (item.PID == PID)
                    {
                        string split = "";
                        for (int i = 0; i < item.Level-1; i++)
                            split += "━";
                        if(item.Level>1)
                            item.Name = "┗" + split + item.Name;
                        resultVideoCategoryInfo.Add(item);
                        ViewCategoryTree(item.ID, videoCategoryInfos, resultVideoCategoryInfo);
                    }
                }
            }
            return resultVideoCategoryInfo;
        }

    }
}
