// SamplePlayClientDlg.h : header file
//

#pragma once
#include "afxwin.h"


// CSamplePlayClientDlg dialog
class CSamplePlayClientDlg : public CDialog
{
// Construction
public:
	CSamplePlayClientDlg(CWnd* pParent = NULL);	// standard constructor
	void RegistVideo();
// Dialog Data
	enum { IDD = IDD_SAMPLEPLAYCLIENT_DIALOG };

private:
	int connectedServerID;
	int camSession;
	char *hostip;
	char *username;
	char *password;
	char *m_processIdPath;
	bool m_bFullScreen;
	bool m_injectApp; //1为正常app 0为嵌入 
	int m_aimCamSeesion;
	WICRect m_originRect;
	WICRect m_fullScreenRect;
	protected:
	virtual void DoDataExchange(CDataExchange* pDX);	// DDX/DDV support

	void Connect();
	void ButtonPlay1();
	void ButtonStop1();
	void ChangeWindow();
// Implementation
protected:
	HICON m_hIcon;



	// Generated message map functions
	virtual BOOL OnInitDialog();
	afx_msg void OnPaint();
	afx_msg HCURSOR OnQueryDragIcon();
	afx_msg void OnDestroy();
	DECLARE_MESSAGE_MAP()
public:
	CListBox listBoxCameras;
	CButton checkBoxEnableSaving;
public:
	afx_msg void OnSize(UINT nType, int cx, int cy);
	afx_msg void OnLButtonDblClk(UINT nFlags, CPoint point);
	afx_msg void OnBnClickedButton1();
	afx_msg void OnContextMenu(CWnd* /*pWnd*/, CPoint /*point*/);
	afx_msg void OnMenuSave();
	afx_msg void OnMenuQuit();
};
