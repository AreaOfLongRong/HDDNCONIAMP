// SamplePlayClientDlg.cpp : implementation file
//

#include "stdafx.h"
#include "SamplePlayClient.h"
#include "SamplePlayClientDlg.h"
#include "ControlCommand.h"
#include <iostream>
#include <fstream>
#include <cstdlib>

//add this definition to use the server-oriented functions.
#define MODE_NETWORK_SERVER 

#include "PlaybackEngine.h"
#include <tchar.h>


#ifdef _DEBUG
#define new DEBUG_NEW
#endif


void UIEventCallBackHandler(MP_ENG_EVENT event, int nIndex, void *pParam, void *pAppData);

// CSamplePlayClientDlg dialog

CSamplePlayClientDlg::CSamplePlayClientDlg(CWnd* pParent /*=NULL*/)
	: CDialog(CSamplePlayClientDlg::IDD, pParent)
{
	m_hIcon = AfxGetApp()->LoadIcon(IDR_MAINFRAME);
}

void CSamplePlayClientDlg::RegistVideo()
{
	CPlaybackEngine::Initialize("{D79399DA-2F36-4f7d-846A-292C90BA9E8D}");
}

void CSamplePlayClientDlg::DoDataExchange(CDataExchange* pDX)
{
	CDialog::DoDataExchange(pDX);
}

BEGIN_MESSAGE_MAP(CSamplePlayClientDlg, CDialog)
	ON_WM_PAINT()
	ON_WM_QUERYDRAGICON()
	ON_WM_DESTROY()
	//}}AFX_MSG_MAP
	
	ON_WM_SIZE()
	ON_WM_LBUTTONDBLCLK()
	ON_BN_CLICKED(IDC_BUTTON1, &CSamplePlayClientDlg::OnBnClickedButton1)
	ON_WM_CONTEXTMENU()
	ON_COMMAND(ID_MENU_32771, &CSamplePlayClientDlg::OnMenuSave)
	ON_COMMAND(ID_MENU_32772, &CSamplePlayClientDlg::OnMenuQuit)
END_MESSAGE_MAP()




// CSamplePlayClientDlg message handlers

BOOL CSamplePlayClientDlg::OnInitDialog()
{
	CDialog::OnInitDialog();

	int x = 0,fx = 0;
	int y = 0, fy = 0;
	int width = 500, fwidth = GetSystemMetrics(SM_CXSCREEN);
	int height = 300, fheight = GetSystemMetrics(SM_CYSCREEN);
	m_injectApp = 1;
	m_bFullScreen = false;
	if (__argc == 15)
	{
		int i = 1;
		hostip = __argv[i++]; //ip 192.168.0.61
		username = __argv[i++]; //username admin
		password = __argv[i++]; //password admin
		m_aimCamSeesion = atoi(__argv[i++]); //id eg.26093
		m_injectApp = atoi(__argv[i++]); //1为正常dialog 0为嵌入无边框
		m_bFullScreen = atoi(__argv[i++]); //1全屏启动 0非全屏

		x = atoi(__argv[i++]); //x
		y = atoi(__argv[i++]);	// y
		width = atoi(__argv[i++]); //width
		height = atoi(__argv[i++]);//height

		fx = atoi(__argv[i++]); //fx
		fy = atoi(__argv[i++]);	//fy
		fwidth = atoi(__argv[i++]); //fwidth
		fheight = atoi(__argv[i++]);//fheight

		if (m_injectApp == 0 && m_bFullScreen == 1)
		{

			char filePath[MAX_PATH];
			GetModuleFileName(NULL, filePath, MAX_PATH); // 得到当前执行文件的文件名（包含路径）
			*(strrchr(filePath, '\\')) = '\0';   // 删除文件名，只留下目录

			strcat(filePath, "\\processId.txt");

			fstream examplefile(filePath);
			if (examplefile.is_open())
			{
				DWORD pid;
				examplefile >> pid;
				HANDLE process = OpenProcess(PROCESS_ALL_ACCESS, FALSE, pid);
				TerminateProcess(process, 4);
			}
			examplefile.close();

			remove(filePath);

			FILE *f = fopen(filePath, "w+");
			DWORD pid = GetCurrentProcessId();
			char buffer[50];
			sprintf(buffer, "%d", pid);
			fputs(buffer, f);
			fclose(f);
		}
		if (m_injectApp)
			m_bFullScreen = false;
	}

	m_originRect.Y = y;
	m_originRect.X = x;
	m_originRect.Width = width;
	m_originRect.Height = height;

	m_fullScreenRect.Y = fy;
	m_fullScreenRect.X = fx;
	m_fullScreenRect.Width = fwidth;
	m_fullScreenRect.Height = fheight;

	ChangeWindow();

	// Set the icon for this dialog.  The framework does this automatically
	//  when the application's main window is not a dialog
	SetIcon(m_hIcon, TRUE);			// Set big icon
	SetIcon(m_hIcon, FALSE);		// Set small icon

	//call these method before everything goes.
	
	CPlaybackEngine* player1 = CPlaybackEngine::getInstance(0);

	player1->SetCallbackFunc(UIEventCallBackHandler, NULL);

	player1->SetVideoAutoScale(false);
	player1->SetDisplayHDC(GetDC()->m_hDC);
	player1->SetLantencyTime(1);
	player1->SetFileNamePrefix("video_save");
	player1->EnableTextOverlap(false, false);
	player1->SetFileSaveType(0);//1 avi,0 crv 
	player1->Load();

	Connect();
	ButtonPlay1();

	return TRUE;  // return TRUE  unless you set the focus to a control
}

void CSamplePlayClientDlg::ChangeWindow()
{

	if (m_injectApp == 1)//dialog
	{
		ModifyStyle(WS_CAPTION | WS_POPUP | WS_SYSMENU | WS_CLIPSIBLINGS | DS_MODALFRAME,
			WS_CAPTION | WS_POPUP | WS_SYSMENU | WS_CLIPSIBLINGS | DS_MODALFRAME, NULL);
		ModifyStyleEx(WS_EX_DLGMODALFRAME | WS_EX_WINDOWEDGE, WS_EX_DLGMODALFRAME | WS_EX_WINDOWEDGE, NULL);
		if (!m_bFullScreen)
		{
			CWnd::SetWindowPos(NULL, m_originRect.X, m_originRect.Y, m_originRect.Width, m_originRect.Height + 40, 0);
			
		}
		else {
			CWnd::SetWindowPos(NULL, m_fullScreenRect.X, m_fullScreenRect.Y, m_fullScreenRect.Width, m_fullScreenRect.Height, 0);
		}
	}
	else//none
	{
		ModifyStyle(WS_CAPTION | WS_POPUP | WS_SYSMENU | WS_CLIPSIBLINGS | DS_MODALFRAME,
			WS_POPUP | WS_SYSMENU | WS_CLIPSIBLINGS, NULL);
		ModifyStyleEx(WS_EX_DLGMODALFRAME | WS_EX_WINDOWEDGE, NULL, NULL);
		if (!m_bFullScreen)
		{
			CWnd::SetWindowPos(NULL, m_originRect.X, m_originRect.Y, m_originRect.Width, m_originRect.Height, 0);

		}
		else
			CWnd::SetWindowPos(NULL, m_fullScreenRect.X, m_fullScreenRect.Y, m_fullScreenRect.Width, m_fullScreenRect.Height, 0);
		
	}
}

// If you add a minimize button to your dialog, you will need the code below
//  to draw the icon.  For MFC applications using the document/view model,
//  this is automatically done for you by the framework.

void CSamplePlayClientDlg::OnPaint()
{
	CPlaybackEngine::getInstance(0)->RefreshWindow();

	if (IsIconic())
	{
		CPaintDC dc(this); // device context for painting

		SendMessage(WM_ICONERASEBKGND, reinterpret_cast<WPARAM>(dc.GetSafeHdc()), 0);

		// Center icon in client rectangle
		int cxIcon = GetSystemMetrics(SM_CXICON);
		int cyIcon = GetSystemMetrics(SM_CYICON);
		CRect rect;
		GetClientRect(&rect);
		int x = (rect.Width() - cxIcon + 1) / 2;
		int y = (rect.Height() - cyIcon + 1) / 2;

		// Draw the icon
		dc.DrawIcon(x, y, m_hIcon);
	}
	else
	{
		CDialog::OnPaint();
	}
}

// The system calls this function to obtain the cursor to display while the user drags
//  the minimized window.
HCURSOR CSamplePlayClientDlg::OnQueryDragIcon()
{
	return static_cast<HCURSOR>(m_hIcon);
}

void CSamplePlayClientDlg::OnDestroy()
{

	ButtonStop1();
	CCameraMngr* cameraManager = CCameraMngr::getInstance();

	CPlaybackEngine* player1 = CPlaybackEngine::getInstance(0);
	player1->Stop();
	player1->Unload();

	if (m_injectApp == 0 && m_bFullScreen == 1)
	{
		char filePath[MAX_PATH];
		GetModuleFileName(NULL, filePath, MAX_PATH); // 得到当前执行文件的文件名（包含路径）
		*(strrchr(filePath, '\\')) = '\0';   // 删除文件名，只留下目录

		strcat(filePath, "\\processId.txt");
		remove(filePath);
	}

	cameraManager->DisconnectServer(connectedServerID);

	CPlaybackEngine::UnInitialize();
	CCameraMngr::Exit();

	//CPlaybackEngine::releaseInstance(0);
	//CPlaybackEngine::releaseInstance(1);
	//CCameraMngr::releaseInstance();
}

void CSamplePlayClientDlg::Connect()
{
	CCameraMngr* cameraManager = CCameraMngr::getInstance();
	cameraManager->SetCallbackFunc(UIEventCallBackHandler, NULL);

	CAM_SERVER_INFO serverInfo;
	if (cameraManager->ConnectServer(hostip, 8310, username, password, &serverInfo) != MP_ENG_OK)
	{
		AfxMessageBox(_T("connect server failed!"));
		return;
	}

	connectedServerID = serverInfo.nCamServerID;

	//retrieve all cameras in all groups
	CAM_INFO *pCamInfos;
	CAM_GROUP_INFO *pCamGroup;

	int nGroupNum = cameraManager->RetrieveCamGroupArrayFromServer(connectedServerID, &pCamGroup);

	list<CAM_GROUP_INFO>::const_iterator groupIter;
	for (int i = 0; i < nGroupNum; i++)
	{
		int nCameraNum = cameraManager->RetrieveCamArrayFromServer(connectedServerID, pCamGroup[i].nCamGroupID, &pCamInfos);
		camSession = -1;
		int tempSession;
		for (int j = 0; j < nCameraNum; j++)
		{
			if (pCamInfos[j].state == MP_CAM_StateOnline)
			{
				tempSession = pCamInfos[j].nSession;
				if (m_aimCamSeesion == tempSession)
				{
					camSession = m_aimCamSeesion;
					break;
				}
			}
		}
		cameraManager->FreeCamArray(pCamInfos);
	}
	cameraManager->FreeCamGroupArray(pCamGroup);
}


void CSamplePlayClientDlg::ButtonPlay1()
{
	CCameraMngr* cameraManager = CCameraMngr::getInstance();
	CPlaybackEngine* player1 = CPlaybackEngine::getInstance(0);

	cameraManager->AssignPlayer(0, camSession);
	player1->Start();
}

void CSamplePlayClientDlg::ButtonStop1()
{
	CPlaybackEngine* player1 = CPlaybackEngine::getInstance(0);
	player1->Stop();
	player1->RefreshWindow();
}

void UIEventCallBackHandler(MP_ENG_EVENT event, int nIndex, void *pParam, void *pAppData)
{
	TCHAR buffer[256];
	//generally, you check the event param value to determine how to consume the pParam data.
	if (event == MP_EVENT_FPS)
	{
		CStatic* label = (CStatic*)pAppData;
		_stprintf_s(buffer, 256, _T("FPS: %.3f"), *(float*)pParam);
		
	}
	else if (event == MP_EVENT_BINDWIDTH)
	{
		CStatic* label = (CStatic*)pAppData;
		_stprintf_s(buffer, 256, _T("Bind width: %.3f bytes/sec"), *(float*)pParam);
		
	}
	else if (event == MP_EVENT_ACK)
	{
		TRACE1("return: %s", (char*)pParam);
		ControlCommandHelper helper;
		Return buffer[10];
		int len = helper.ParseReturnString((char*)pParam, buffer, sizeof(buffer));
		for (int i = 0; i < len; i++)
		{
			TCHAR info[50];
			_stprintf_s(info, 50, _T("seq %d, val %d\n"), buffer[i].sequence, buffer[i].value);
			AfxMessageBox(info);
		}
	}
	else
	{
		_stprintf_s(buffer, 256, _T("UI event raised: event code %d, index %d, param 0x%X, application data 0x%X.\n"), 
			event, nIndex, (DWORD)pParam, (DWORD)pAppData);
		OutputDebugString(buffer);
		//you can see this information in the output window of the debugger if you 
		//attach one at runtime. Typically, if you debug this program in visual studio,
		//you can get the information in the Output window of VS.
	}
}

void CSamplePlayClientDlg::OnSize(UINT nType, int cx, int cy)
{
	CPlaybackEngine* player1 = CPlaybackEngine::getInstance(0);
	player1->SetDisplayRange(0, 0, cx, cy);

	CDialog::OnSize(nType, cx, cy);
}


void CSamplePlayClientDlg::OnLButtonDblClk(UINT nFlags, CPoint point)
{
	// TODO: 在此添加消息处理程序代码和/或调用默认值
	if (nFlags)
	{
		if (m_injectApp == 0)
		{
			if (m_bFullScreen)
			{
				AfxGetMainWnd()->SendMessage(WM_CLOSE);
			}else
			{
				CString command;
				command.Format(_T("SamplePlayClient.exe %s %s %s %d %d %d %d %d %d %d %d %d %d %d")
					, hostip,username,password,
					m_aimCamSeesion, 0, 1,
					m_originRect.X, m_originRect.Y, m_originRect.Width, m_originRect.Height,
					m_fullScreenRect.X, m_fullScreenRect.Y, m_fullScreenRect.Width, m_fullScreenRect.Height);
				WinExec((LPTSTR)(LPCTSTR)command, SW_SHOW);
			}
		}
		else
		{
			m_bFullScreen = !m_bFullScreen;
			ChangeWindow();
		}
	}
	CDialog::OnLButtonDblClk(nFlags, point);
	
}


void CSamplePlayClientDlg::OnBnClickedButton1()
{
	CString str;
	GetDlgItemText(IDC_BUTTON1, str);
	CPlaybackEngine* player1 = CPlaybackEngine::getInstance(0);
	if (str == "保存")
	{
		player1->EnableFileSaving(true);
		GetDlgItem(IDC_BUTTON1)->SetWindowText(_T("正在保存..."));
	}
	else {
		player1->EnableFileSaving(false);
		GetDlgItem(IDC_BUTTON1)->SetWindowText(_T("保存"));
	}
}


void CSamplePlayClientDlg::OnContextMenu(CWnd* /*pWnd*/, CPoint point)
{
	// TODO: 在此处添加消息处理程序代码
	CMenu menu;

	menu.LoadMenu(IDR_MENU1); //读取资源
	menu.GetSubMenu(0)->TrackPopupMenu(TPM_LEFTALIGN | TPM_LEFTBUTTON | TPM_RIGHTBUTTON, point.x, point.y, this);
}


static CString Show()
{
	TCHAR           szFolderPath[MAX_PATH] = { 0 };
	CString         strFolderPath = TEXT("");

	BROWSEINFO      sInfo;
	::ZeroMemory(&sInfo, sizeof(BROWSEINFO));
	sInfo.pidlRoot = 0;
	sInfo.lpszTitle = _T("请选择一个文件夹：");
	sInfo.ulFlags = BIF_DONTGOBELOWDOMAIN | BIF_RETURNONLYFSDIRS | BIF_NEWDIALOGSTYLE | BIF_EDITBOX;
	sInfo.lpfn = NULL;

	// 显示文件夹选择对话框  
	LPITEMIDLIST lpidlBrowse = ::SHBrowseForFolder(&sInfo);
	if (lpidlBrowse != NULL)
	{
		// 取得文件夹名  
		if (::SHGetPathFromIDList(lpidlBrowse, szFolderPath))
		{
			strFolderPath = szFolderPath;
		}
	}
	if (lpidlBrowse != NULL)
	{
		::CoTaskMemFree(lpidlBrowse);
	}

	return strFolderPath;

}

void CSamplePlayClientDlg::OnMenuSave()
{
	// TODO: 在此添加命令处理程序代码
	CPlaybackEngine* player1 = CPlaybackEngine::getInstance(0);

	//open file dialog
	CString path = Show();
	if (path == TEXT(""))
	{
		return;
	}
	else {
		player1->EnableFileSaving(false);
		char *c_path = path.GetBuffer();
		player1->SetFileSavingPath(c_path);
		player1->EnableFileSaving(true);
		GetDlgItem(ID_MENU_32771)->SetWindowText(_T("结束保存"));
	}
	

}


void CSamplePlayClientDlg::OnMenuQuit()
{
	// TODO: 在此添加命令处理程序代码
	DestroyWindow();
}
