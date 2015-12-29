/*
*	门户展现数据，初始化范围包括子系统、菜单组、菜单项,左侧导航，菜单深度5级
*   配置通过初始化systemJson、menuGroupJson、menuJson、leftMenuBarJson、leftMenuJson五个变量完成，其余方法勿动！
*   LEFTPAGEURL：控制左侧是否出现边栏，PAGEURL：对于子系统是系统首页，对于菜单组是菜单组的首页，对菜单则是该菜单的功能入口
*   SELECTED:配置是否默认选中
*	liuyong.2012-10
*/

//子系统数据
var systemJson=[
				{"UUID":"A0","NAME":"系统管理员"},
				{"UUID":"A1","NAME":"审计管理员","SELECTED":true},
				{"UUID":"A2","NAME":"公司审计组长"},
				{"UUID":"A4","NAME":"公司审计专员"},
				{"UUID":"A5","NAME":"被审计单位领导"},
				{"UUID":"A3","NAME":"被审计单位迎审专员"}
				];
//菜单组数据
var menuGroupJson=[
				   //{"UUID":"B9","NAME":"首页","PID":"A0","SELECTED":true,"PAGEURL":"admin/index_temp.html"},
				   {"UUID":"B1","NAME":"组织管理","PID":"A0","SELECTED":true,"PAGEURL":"admin/ad_zzjg.html"},
				   {"UUID":"B1","NAME":"角色权限管理","PID":"A0","PAGEURL":"admin/ad_jsgl.html"},
				   {"UUID":"B1","NAME":"人员管理","PID":"A0","PAGEURL":"admin/ad_rygl.html"},
				   {"UUID":"B7","NAME":"资源共享","PID":"A0","PAGEURL":"share/sh_aqzygx.html"},
				   {"UUID":"B8","NAME":"交流平台","PID":"A0","PAGEURL":"share/sh_aqjlpt.html"},
				   {"UUID":"B9","NAME":"互助直通车","PID":"A0","PAGEURL":"share/sh_hzztc.html"},
				   
				   {"UUID":"B9","NAME":"首页","PID":"A1","SELECTED":true,"PAGEURL":"admin/index.html"},
				   {"UUID":"B1","NAME":"审计单类型管理","PID":"A1","PAGEURL":"admin/ad_sjdlx.html"},
				   {"UUID":"B1","NAME":"审计要素管理","PID":"A1","PAGEURL":"admin/ad_sjys.html"},
				   {"UUID":"B1","NAME":"问题分类管理","PID":"A1","PAGEURL":"admin/ad_wtfl.html"},
				   {"UUID":"B2","NAME":"专业系数管理","PID":"A1","PAGEURL":"admin/ad_zyqz.html"},
				   {"UUID":"B1","NAME":"风险指标管理","PID":"A1","PAGEURL":"admin/ad_fxzb.html"},
				   {"UUID":"B6","NAME":"安全现状监控","PID":"A1","PAGEURL":"analysis/an_aqxzjk.html"},
				   {"UUID":"B6","NAME":"统计分析","PID":"A1","PAGEURL":"analysis/an_zhtjfx.html"},
				   {"UUID":"B7","NAME":"资源共享","PID":"A1","PAGEURL":"share/sh_aqzygx.html"},
				   {"UUID":"B8","NAME":"交流平台","PID":"A1","PAGEURL":"share/sh_aqjlpt.html"},
				   {"UUID":"B9","NAME":"互助直通车","PID":"A1","PAGEURL":"share/sh_hzztc.html"},
				   {"UUID":"B9","NAME":"首页","PID":"A2","SELECTED":true,"PAGEURL":"audit/leader_index.html"},
				   {"UUID":"B3","NAME":"审计计划","PID":"A2","PAGEURL":"audit/au_sjjh.html"},
				   {"UUID":"B1","NAME":"审计要素重要性","PID":"A2","PAGEURL":"admin/ad_sjdzyx.html"},
				   {"UUID":"B3","NAME":"系统构型","PID":"A2","PAGEURL":"audit/au_xtgx.html"},
				   {"UUID":"B3","NAME":"专项审计","PID":"A2","PAGEURL":"audit/au_zxsj.html"},
				   {"UUID":"B3","NAME":"复核验证","PID":"A2","PAGEURL":"audit/au_fhyz.html"},
				   {"UUID":"B3","NAME":"预审/审计","PID":"A2","PAGEURL":"audit/au_sjsj.html"},
				   {"UUID":"B5","NAME":"分析评估","PID":"A2","PAGEURL":"audit/au_fxpg.html"},
				   {"UUID":"B5","NAME":"审计报告","PID":"A2","PAGEURL":"audit/au_sjbg.html"},
				   {"UUID":"B5","NAME":"行动跟踪","PID":"A2","PAGEURL":"audit/au_yfjz.html"},
				   {"UUID":"B6","NAME":"统计分析","PID":"A2","PAGEURL":"analysis/an_zhtjfx.html"},
				   {"UUID":"B7","NAME":"资源共享","PID":"A2","PAGEURL":"share/sh_aqzygx.html"},
				   {"UUID":"B8","NAME":"交流平台","PID":"A2","PAGEURL":"share/sh_aqjlpt.html"},
				   {"UUID":"B9","NAME":"互助直通车","PID":"A2","PAGEURL":"share/sh_hzztc.html"},
				   {"UUID":"B9","NAME":"首页","PID":"A4","SELECTED":true,"PAGEURL":"audit/index.html"},
				   {"UUID":"B1","NAME":"审计单重要性","PID":"A4","PAGEURL":"admin/ad_sjdzyx.html"},
				   {"UUID":"B3","NAME":"系统构型","PID":"A4","PAGEURL":"audit/au_xtgx_b.html"},
				   {"UUID":"B3","NAME":"专项审计","PID":"A4","PAGEURL":"audit/au_zxsj.html"},
				   {"UUID":"B3","NAME":"复核验证","PID":"A4","PAGEURL":"audit/au_fhyz.html"},
				   {"UUID":"B3","NAME":"预审/审计","PID":"A4","PAGEURL":"audit/au_sjsj.html"},
				   {"UUID":"B5","NAME":"分析评估","PID":"A4","PAGEURL":"audit/au_fxpg.html"},
				   {"UUID":"B5","NAME":"审计报告","PID":"A4","PAGEURL":"audit/au_sjbg.html"},
				   {"UUID":"B5","NAME":"行动跟踪","PID":"A4","PAGEURL":"audit/au_yfjz.html"},
				   {"UUID":"B6","NAME":"统计分析","PID":"A4","PAGEURL":"analysis/an_zhtjfx.html"},
				   {"UUID":"B7","NAME":"资源共享","PID":"A4","PAGEURL":"share/sh_aqzygx.html"},
				   {"UUID":"B8","NAME":"交流平台","PID":"A4","PAGEURL":"share/sh_aqjlpt.html"},
				   {"UUID":"B9","NAME":"互助直通车","PID":"A4","PAGEURL":"share/sh_hzztc.html"},
				   {"UUID":"B9","NAME":"首页","PID":"A3","SELECTED":true,"PAGEURL":"enterprise/index.html"},
				   {"UUID":"B4","NAME":"数据收集","PID":"A3","PAGEURL":"enterprise/ep_sjsj_b.html"},
				   //{"UUID":"B2","NAME":"规章文件","PID":"A3","PAGEURL":"audit/au_gzwj.html"},
				   {"UUID":"B6","NAME":"后续行动","PID":"A3","PAGEURL":"enterprise/ep_hxxd_b.html"},
				   {"UUID":"B6","NAME":"统计分析","PID":"A3","PAGEURL":"analysis/an_tjfx.html"},
				   {"UUID":"B7","NAME":"资源共享","PID":"A3","PAGEURL":"share/sh_aqzygx.html"},
				   {"UUID":"B8","NAME":"交流平台","PID":"A3","PAGEURL":"share/sh_aqjlpt.html"},
				   {"UUID":"B9","NAME":"互助直通车","PID":"A3","PAGEURL":"share/sh_hzztc.html"},
				   {"UUID":"B9","NAME":"首页","PID":"A5","SELECTED":true,"PAGEURL":"enterprise/leader_index.html"},
				   //{"UUID":"B3","NAME":"审计计划","PID":"A5","PAGEURL":"enterprise/ep_sjsj.html"},
				   //{"UUID":"B5","NAME":"审计报告","PID":"A5","PAGEURL":"enterprise/ep_sjbg.html"},
				   {"UUID":"B4","NAME":"数据收集","PID":"A5","PAGEURL":"enterprise/ep_sjsj.html"},
				   //{"UUID":"B2","NAME":"规章文件","PID":"A5","PAGEURL":"audit/au_gzwj.html"},
				   {"UUID":"B6","NAME":"后续行动","PID":"A5","PAGEURL":"enterprise/ep_hxxd.html"},
				   {"UUID":"B6","NAME":"统计分析","PID":"A5","PAGEURL":"analysis/an_zhtjfx.html"},
				   {"UUID":"B7","NAME":"资源共享","PID":"A5","PAGEURL":"share/sh_aqzygx.html"},
				   {"UUID":"B8","NAME":"交流平台","PID":"A5","PAGEURL":"share/sh_aqjlpt.html"},
				   {"UUID":"B9","NAME":"互助直通车","PID":"A5","PAGEURL":"share/sh_hzztc.html"}
				   ];
//菜单数据
var menuJson=[
			  //{"UUID":"C1","NAME":"查询列表示例","PID":"B1","SELECTED":true,"PAGEURL":"demo/查询+列表示例.html"},
			  //{"UUID":"C2","NAME":"菜单2","PID":"B7"},
//			  {"UUID":"C3","NAME":"菜单3","PID":"B8","SELECTED":true},
//			  {"UUID":"C4","NAME":"综合示例","PID":"B1","LEFTPAGEURL":"leftFrame.html"},
//			  {"UUID":"C5","NAME":"菜单5","PID":"B1"},
//			  {"UUID":"C6","NAME":"菜单6","PID":"B1"},
//			  {"UUID":"C7","NAME":"菜单5","PID":"B2"},
//			  {"UUID":"C8","NAME":"菜单6","PID":"B2","SELECTED":true},
//			  {"UUID":"C9","NAME":"权限管理","PID":"B6","SELECTED":true,"LEFTPAGEURL":"leftFrame.html"}
			  ];

//左侧导航一级菜单
var leftMenuBarJson=[
			  //{"UUID":"D1","NAME":"页面模版","PID":"C4"},
//			  {"UUID":"D2","NAME":"其他","PID":"C4"},
//			  {"UUID":"D3","NAME":"权限管理","PID":"C9"}
			  ];

//左侧导航二级菜单
var leftMenuJson=[
			  {"UUID":"E1","NAME":"查询+列表示例","PID":"D1","SELECTED":true,"PAGEURL":"demo/查询+列表示例.html"},
			  {"UUID":"E12","NAME":"统计+列表示例","PID":"D1","PAGEURL":"demo/统计+列表示例.html"},
			  {"UUID":"E3","NAME":"多条件查询+列表示例","PID":"D1","PAGEURL":"demo/多条件查询+列表示例.html"},
			  {"UUID":"E4","NAME":"信息填报页示例","PID":"D1","PAGEURL":"demo/信息填报页示例.html"},
			  {"UUID":"E5","NAME":"信息填报页分组示例","PID":"D1","PAGEURL":"demo/信息填报页分组示例.html"},
			  {"UUID":"E6","NAME":"信息展现示例","PID":"D1","PAGEURL":"demo/信息展现示例.html"},
			  {"UUID":"E7","NAME":"横向标签页示例","PID":"D1","PAGEURL":"demo/横向标签页示例.html"},
			  {"UUID":"E8","NAME":"弹出窗口示例","PID":"D1","PAGEURL":"demo/弹出窗口示例.html"},
			  {"UUID":"E9","NAME":"树示例","PID":"D1","PAGEURL":"demo/树示例.html"},
			  {"UUID":"E10","NAME":"表单元素","PID":"D2","PAGEURL":"#"},
			  {"UUID":"E11","NAME":"其他菜单2","PID":"D2","PAGEURL":"#"},
			  {"UUID":"E12","NAME":"组织管理","PID":"D3","PAGEURL":"system/orgNav.html"},
			  {"UUID":"E13","NAME":"人员管理","PID":"D3","PAGEURL":"system/personNav.html"},
			  {"UUID":"E14","NAME":"角色管理","PID":"D3","PAGEURL":"system/roleList.html"},
			  {"UUID":"E15","NAME":"用户组管理","PID":"D3","PAGEURL":"system/usergroupList.html"},
			  {"UUID":"E16","NAME":"功能资源管理","PID":"D3","PAGEURL":"system/resourceNav.html"}
			  ];


//--------------------------------------------------------下面的就不要动咯--------------------------------------------------
//------------------------------------------------------------------------------------------------------------------------
//配置是否显示二级菜单   "true"始终显示，"false"始终不显示，"auto"自动根据是否有超过一个的自菜单进行判断，有显示，没有不显示
var showChildMenu="auto";
//带二级菜单菜单FRAME高度
var withChildMenuHeight=120;
//不带二级菜单菜单FRAME高度
var noChildMenuHeight=97;
//当前生效或点中的菜单，leftFrame中用到
var currentNavObjectId=null;

$(function(){
	if(showChildMenu=="true"){
		changeTopAreaHeight(withChildMenuHeight);
	}else if(showChildMenu=="false"){
		changeTopAreaHeight(noChildMenuHeight);
	}
	//初始化子系统
	iniSystem();
	//即使显示二级菜单，但如果二级菜单是空的话，那么也不显示相关区域
	if(showChildMenu=="auto" && $(".yChildMenu").length<=1){
		changeTopAreaHeight(noChildMenuHeight);
	}
});

//初始化子系统
function iniSystem(){
	$("#yMenuGroup").html("");
	$("#yChildMenuGroup").html("");
	for(var i=0;i<systemJson.length;i++){
		if(systemJson[i].SELECTED){
			doAction(0,systemJson[i].UUID,systemJson[i].LEFTPAGEURL,systemJson[i].PAGEURL);
			$("<option selected='selected' value='"+systemJson[i].UUID+"'>"+systemJson[i].NAME+"</option>").appendTo($("#ySystemGroupSelect"));
			iniMenuGroup(systemJson[i].UUID);
		}else{
			$("<option value='"+systemJson[i].UUID+"'>"+systemJson[i].NAME+"</option>").appendTo($("#ySystemGroupSelect"));
		}
	}
	
}

//初始化菜单组
function iniMenuGroup(UUID){
	$("#yMenuGroup").html("");
	$("#yChildMenuGroup").html("");
	for(var i=0;i<menuGroupJson.length;i++){
		if(menuGroupJson[i].PID==UUID){
			if(menuGroupJson[i].SELECTED){
				doAction(1,menuGroupJson[i].UUID,menuGroupJson[i].LEFTPAGEURL,menuGroupJson[i].PAGEURL);
				var tem=$("<div class='yTopMenu yTopMenuOn' id='"+menuGroupJson[i].UUID+"'><div>"+menuGroupJson[i].NAME+"</div></div>");
				tem.bind("click",menuGroupJson[i],function(event){
					$("#yChildMenuGroup").html("");
					doAction(1,event.data.UUID,event.data.LEFTPAGEURL,event.data.PAGEURL);
					iniMenu(event.data.UUID);			  
				});
				tem.appendTo($("#yMenuGroup"));	
				iniMenu(menuGroupJson[i].UUID);
			}else{
				var tem=$("<div class='yTopMenu' id='"+menuGroupJson[i].UUID+"'><div>"+menuGroupJson[i].NAME+"</div></div>");	
				tem.bind("click",menuGroupJson[i],function(event){
					$("#yChildMenuGroup").html("");
					doAction(1,event.data.UUID,event.data.LEFTPAGEURL,event.data.PAGEURL);
					iniMenu(event.data.UUID);			  
				});
				tem.appendTo($("#yMenuGroup"));	
			}
		}
	}
	//如果不这样，appendTo进去的元素在IE8下不执行JS方法
	//$("#yMenuGroup").html($("#yMenuGroup").html());
}

//初始化菜单
function iniMenu(UUID){
	$("#yChildMenuGroup").html("");
	for(var i=0;i<menuJson.length;i++){
		if(menuJson[i].PID==UUID){
			if(menuJson[i].SELECTED){
				doAction(2,menuJson[i].UUID,menuJson[i].LEFTPAGEURL,menuJson[i].PAGEURL);
				var tem=$("<div class='yChildMenu yChildMenuOn' id='"+menuJson[i].UUID+"'><div>"+menuJson[i].NAME+"</div></div>");
				tem.bind("click",menuJson[i],function(event){
					doAction(2,event.data.UUID,event.data.LEFTPAGEURL,event.data.PAGEURL);		  
				});
				tem.appendTo($("#yChildMenuGroup"));	
			}else{
				var tem=$("<div class='yChildMenu' id='"+menuJson[i].UUID+"'><div>"+menuJson[i].NAME+"</div></div>");	
				tem.bind("click",menuJson[i],function(event){
					doAction(2,event.data.UUID,event.data.LEFTPAGEURL,event.data.PAGEURL);		  
				});
				tem.appendTo($("#yChildMenuGroup"));	
			}
		}
	}
	
	//即使显示二级菜单，但如果二级菜单是空的话，那么也不显示相关区域
	if(showChildMenu=="auto" && $(".yChildMenu").length<=1){
		changeTopAreaHeight(noChildMenuHeight);
	}else if(showChildMenu=="auto" && $(".yChildMenu").length>1){
		changeTopAreaHeight(withChildMenuHeight);
	}
	
	//如果不这样，appendTo进去的元素在IE8下不执行JS方法
	//$("#yChildMenuGroup").html($("#yChildMenuGroup").html());
}

//控制左中右frame的显示隐藏及url
//type标示该动作来自第几层菜单
function doAction(type,objectId,leftUrl,pageUrl){
	var hasRightIniSrcFlag=false;
	if(pageUrl!=null && pageUrl !="undefined" && pageUrl !="null"){
		currentNavObjectId=objectId;
		window.parent.$("#mainFrame").attr("src",pageUrl);
		hasRightIniSrcFlag=true;
	}
	if(leftUrl!=null && leftUrl !="undefined" && leftUrl !="null"){
		currentNavObjectId=objectId;
		window.parent.$("#leftFrameTD").show();	
		window.parent.$("#controlFrameTD").show();
		window.parent.$("#leftFrame").attr("src",leftUrl);
		if(!hasRightIniSrcFlag){
			window.parent.$("#mainFrame").attr("src","");
		}
	}else{
		window.parent.$("#leftFrameTD").hide();	
		window.parent.$("#leftFrameTD").attr("src","");
		window.parent.$("#controlFrameTD").hide();	
	}
}

//显示或影藏二级菜单
function changeTopAreaHeight(num){
	window.parent.$("#menuFrameTD").height(num);
	window.parent.$("#menuFrame").height(num);
	window.parent.adaptWindowHeight();
}