// GetVideoInfo.cpp : 定义控制台应用程序的入口点。
//


#include "stdafx.h"
#include "PlaybackEngine.h"

int main(int argc, char *argv[])
{
	char *ipAddr = "", *loginName = "", *loginpwd = "";
	int m_aimCamSeesion = -1;

	if (argc == 5)
	{
		int i = 1;
		m_aimCamSeesion = atoi(argv[i++]);
		ipAddr = argv[i++];
		loginName = argv[i++];
		loginpwd = argv[i++];
	}
	else
	{
		printf("argc is not correct");
	}
	CPlaybackEngine::Initialize("{D79399DA-2F36-4f7d-846A-292C90BA9E8D}");
	CCameraMngr* cameraManager = CCameraMngr::getInstance();

	CAM_SERVER_INFO serverInfo;
	if (cameraManager->ConnectServer(ipAddr, 8310, loginName, loginpwd, &serverInfo) != MP_ENG_OK)
	{
		return -1;
	}

	int connectedServerID = serverInfo.nCamServerID;

	//retrieve all cameras in all groups
	CAM_INFO *pCamInfos;
	CAM_GROUP_INFO *pCamGroup;

	int nGroupNum = cameraManager->RetrieveCamGroupArrayFromServer(connectedServerID, &pCamGroup);

	int result = -1;
	list<CAM_GROUP_INFO>::const_iterator groupIter;
	for (int i = 0; i < nGroupNum; i++)
	{
		int nCameraNum = cameraManager->RetrieveCamArrayFromServer(connectedServerID, pCamGroup[i].nCamGroupID, &pCamInfos);

		int tempSession;
		for (int j = 0; j < nCameraNum; j++)
		{
			if (pCamInfos[j].state == MP_CAM_StateOnline)
			{
				tempSession = pCamInfos[j].nSession;
				if (m_aimCamSeesion == tempSession)
				{
					result = 1;
					break;
				}
			}
		}
		cameraManager->FreeCamArray(pCamInfos);
	}
	cameraManager->FreeCamGroupArray(pCamGroup);
    return result;
}

