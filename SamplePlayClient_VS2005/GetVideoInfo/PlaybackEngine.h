// The following ifdef block is the standard way of creating macros which make exporting 
// from a DLL simpler. All files within this DLL are compiled with the PLAYBACKENGINE_EXPORTS
// symbol defined on the command line. this symbol should not be defined on any project
// that uses this DLL. This way any other project whose source files include this file see 
// PLAYBACKENGINE_API functions as being imported from a DLL, wheras this DLL sees symbols
// defined with this macro as being exported.

#define MODE_NETWORK_SERVER

#if !defined(AFX_PLAYBACKENGINE_H__E213B48A_5511_4104_B474_A4C2779BB995__INCLUDED_)
#define AFX_PLAYBACKENGINE_H__E213B48A_5511_4104_B474_A4C2779BB995__INCLUDED_

#ifdef PLAYBACKENGINE_EXPORTS
#define PLAYBACKENGINE_API __declspec(dllexport)
#else
#ifdef PLAYBACKENGINE_STATIC
#define PLAYBACKENGINE_API
#else
#define PLAYBACKENGINE_API __declspec(dllimport)
#endif
#endif

#include <list>
using namespace std;

//////////////////////////////////////////////////////
///Error Code
#define MP_ENG_OK              0     //success
#define MP_ENG_ERROR           -1    //general error
#define MP_ENG_INVALID_PARAM   -2    //parameter invalid
#define MP_ENG_INVALID_STATE   -3    //engine state invalid
#define MP_ENG_PERM_DENY       -4    //authenication error
#define MP_ENG_BUFFER_OVERFLOW -5    //buffer overflow error
#define MP_ENG_NO_DATA         -6    //Can not get data 
#define MP_ENG_CONNECT_FAILED  -7    //connect server error
#define MP_ENG_DUP_SERVER      -8    //login to a duplicated server

//Engine state
#define MP_ENG_StateInvalid    -1    
#define MP_ENG_StateInit       0     
#define MP_ENG_StateLoaded     1
#define MP_ENG_StateExecuting  2  
#define MP_ENG_StatePaused     3  

//Connect mode
#define MP_PROTO_UDP 1
#define MP_PROTO_TCP 2

//////////////////////////////////////////////////////
class CMemBufferPool;
class RTPPacketMux;
class H264Decode;
class PostProcess;
class Render;
class G7231Decode;
class AudioRender;

//mode of playback engine
enum MP_ENG_MODE
{
    MP_MODE_NET = 0x01,            //network rtp living mode
    MP_MODE_LOCAL = 0x02,          //local player mode
    MP_MODE_SERV_PROXY = 0x04,     //server proxy mode
    MP_MODE_DEC_YUVDATA = 0x08     //retrieve raw data from decoder
};

//////////////////////////////////////////////////////
//UI Callback Function define
enum MP_ENG_EVENT
{
    MP_EVENT_FPS,                   //frame per second changed
    MP_EVENT_BINDWIDTH,             //network bindwidth changed
    MP_EVENT_PLR,                   //packet loss rate
    MP_EVENT_ERROR,
    MP_EVENT_ACK,                   //ack feedback message from network, after caller send a cmd to camera
    MP_EVENT_CAMJOIN,               //new camera join the network
    MP_EVENT_CAMEXIT,               //camera disconnect
    MP_EVENT_WATERMARK_LOW,         //this event happens when decode buffer is low, in local play mode    
    MP_EVENT_WATERMARK_HIGH,        //this event happens when decode buffer is high, in local play mode   
    
    MP_EVENT_USER_LOGIN,            //user login to server
    MP_EVENT_USER_LOGOUT,           //user logout
    MP_EVENT_USER_PWD_ERROR,        //user enter password error
    MP_EVENT_SERVER_DISCONNECT,     //server disconnect
    MP_EVENT_SERVER_CONNECT,        //server connect
    MP_EVENT_MOVE_DETECTION,        //move event
    MP_EVENT_MOVE_STOP,             //move stop
    MP_EVENT_ALARM,                 //alarm occurs
    MP_EVENT_DISK_LOW,              //disk space low
    MP_EVENT_DISK_FULL,             //disk space full

    MP_EVENT_DATA_READY             //Video,audio or other data ready
};

//MP_EVENT_DATA_READY parameter define
//
enum MP_DATA_TYPE
{
    MP_DATA_H264,            //video data
    MP_DATA_G729,            //audio data
    MP_DATA_G723,            //audio data
    MP_DATA_YUV,             //yuv420 data
    MP_DATA_PCM,             //pcm raw data, 8k 16bits
    MP_DATA_UART             //uart data
};

typedef struct _MP_DATA_INFO 
{
    MP_DATA_TYPE type;
    unsigned char *pData;
    int nLen;   //length of pData
    int nFlag;
    int nTimestamp;
    int nTimestamp2;
    int nSeqNum; 
} MP_DATA_INFO;

typedef struct _MP_YUV_INFO 
{
    int nWidth;
    int nHeight;
    unsigned char *pY;
    unsigned char *pU;
    unsigned char *pV;
    const char *szTime;   //time stamp
} MP_YUV_INFO;

//UI part should implement this callback, to wait engine event notification
//parameter, nIndex -- when from CCameraMngr, it is camera session index
//                  -- when from CPlaybackEngine, it is Player index
typedef void (*UIEventCallBack)(MP_ENG_EVENT event, int nIndex, void *pParam, void *pAppData);
/////////////////////////////////////////////////////////

#ifdef MODE_NETWORK_SERVER
#define MP_CAM_StateInvalid    -1    
#define MP_CAM_StateOnline      0     
#define MP_CAM_StateOffline     1
#endif

#define MAX_VIDEO_CHN_NUM 4

typedef struct _CAM_CHANNEL_INFO 
{
    int nChannelID;          //camera channel id
    int nCameraID;
    char szChannelName[64];  //camera channel name   
    int nPlayerIndex;        //the associated player index, will be -1 if not linked

    int state;
} CAM_CHANNEL_INFO;

typedef struct _CAM_INFO 
{
    int nSession;         //camera session id
    char szCameraID[64];  //camera id   
    char szIPAddr[32];    //camera ip address
    int nPlayerIndex[MAX_VIDEO_CHN_NUM];     //the associated player index, will be -1 if not linked

#ifdef MODE_NETWORK_SERVER
    char szLocation[64];
    int nCamGroupID;  //belong to which camera group
    int nServerID;    //belong to which server
    int state;        //MP_CAM_StateXXXX
#endif

} CAM_INFO;

#ifdef MODE_NETWORK_SERVER
typedef struct _CAM_GROUP_INFO 
{
    int nCamGroupID;         //camera group id
    char szGroupName[64];    //camera group name
    int nServerID;           //belong to which server
    int state;               //MP_CAM_StateXXXX
} CAM_GROUP_INFO;

typedef struct _CAM_SERVER_INFO 
{
    int nCamServerID;         //camera server id
    char szCamServerName[64]; //camera server name
    char szIPAddr[32];
    int  nPort;
    int state;                //MP_CAM_StateXXXX
} CAM_SERVER_INFO;
#endif


// This class is exported from the PlaybackEngine.dll
// Camera Management Interface
class PLAYBACKENGINE_API CCameraMngr 
{
public:

    static CCameraMngr *getInstance();
    static void releaseInstance();

    //
    static void Exit();
    
    //set call back to UI
    int SetCallbackFunc(UIEventCallBack pCallbackFunc, void *pAppData);

    //get active camera list, must release after used
    list<CAM_INFO> &GetLiveCameras();
    void ReleaseLiveCameras();
    
    //get live camera copy and release lock immediately
    list<CAM_INFO> GetLiveCamerasAndRelease();

    //assign a player engine to a camera channel
    int AssignPlayer(int nPlayerIndex, int nCameraChannel);

    //get associated player, if none, return -1;
    int GetLinkPlayer(int nCameraChannel);

    //Camera Control
    int SendControlCmd(int nCameraChannel, const char *szCmd);

    //UART Data
    int SendUARTData(int nCameraChannel, unsigned char *pData, int nLen);

	//Get gps info 
	int GetGPSInfo(int nCameraChannel, int *Latitude, int *Longitude); 

#ifdef MODE_NETWORK_SERVER
    //connect to streaming server, if succeed, pServerInfo will be filled with server information
    int ConnectServer(const char* serverIpAddr, 
                      int serverPort, 
                      const char* userName, 
                      const char* password,
                      CAM_SERVER_INFO *pServerInfo,
                      int connect_mode = MP_PROTO_TCP);

    int DisconnectServer(int nServerID);

    int RetrieveCamGroupListFromServer(int nServerID, list<CAM_GROUP_INFO> *pCamGroupList);
    void FreeCamGroupList(list<CAM_GROUP_INFO> &camGroupList);

    int RetrieveCamListFromServer(int nServerID, int nGroupID, list<CAM_INFO> *pCamInfoList);
    void FreeCamList(list<CAM_INFO> &camInfoList);
    
    int RetrieveCamChannelListFromServer(int nServerID, int nCameraID, list<CAM_CHANNEL_INFO> *pCamChnInfoList);
    void FreeCamChannelList(list<CAM_CHANNEL_INFO> &camChnInfoList);

    //For VS 2005 ++ version
	int RetrieveCamGroupArrayFromServer(int nServerID, CAM_GROUP_INFO **ppCamGroupArray);
    void FreeCamGroupArray(CAM_GROUP_INFO *pCamGroupArray);

    int RetrieveCamArrayFromServer(int nServerID, int nGroupID, CAM_INFO **ppCamInfoArray);
    void FreeCamArray(CAM_INFO *pCamInfoArray);
    
    int RetrieveCamChannelArrayFromServer(int nServerID, int nCameraID, CAM_CHANNEL_INFO **ppCamChnInfoArray);
    void FreeCamChannelArray(CAM_CHANNEL_INFO *pCamChnInfoArray);

#endif

    //End public interface
    //Below not for client use
    //receive event from network
    void EventHandler(MP_ENG_EVENT event, int nChannel, void *pParam);
    
protected:
    CCameraMngr();
    ~CCameraMngr();

private:
    
    static CCameraMngr *m_instance;

    //
    UIEventCallBack m_pCallbackFunc;
    void *m_pAppData;

    CRITICAL_SECTION m_lock;

    friend class CPlaybackEngine;
};

// This class is exported from the PlaybackEngine.dll
// Media Player Engine Interface
class PLAYBACKENGINE_API CPlaybackEngine 
{
public:
    //First of all, caller MUST call Initialize
    //in one process, this should be called only once
    static int Initialize(const char *pParam);
    static void UnInitialize();
    
    //Create Playback Engine using Singleton Pattern
    //Directly create is prohibited
    static CPlaybackEngine *getInstance(int nIndex = 0);
    static void releaseInstance(int nIndex = 0);

    //parameter setting
    int EnableDDraw(bool bEnable, HWND hwnd);
    int SetDisplayHDC(HDC hdc);
    int SetDisplayRange(int x, int y, int nWidth, int nHeight);
    int SetVideoAutoScale(bool bAutoScale);
    int SetLantencyTime(int nMilliSeconds);
        
    //load will enter loaded state,
    //unload will return to init state
    int Load();
    int Unload();

    //start will enter executing state
    //and stop will return to loaded state
    int Start();
    int Stop();
    
    //pause or resume video/audio process 
    int Pause();
    int Resume();

    //Call refresh when windows switch
    int RefreshWindow();

    //Engine state
    int GetState() { return m_state; };

    //call back to UI
    int SetCallbackFunc(UIEventCallBack pCallbackFunc, void *pAppData);
    
    //For local player
    //default is network mode, so localplayer must call this interface 
    int SetMode(enum MP_ENG_MODE mode);
    
    //Feed data from localplayer to playback engine
    int FeedLocalVideoData(const char *pDataBuf, int nLen);

    //Set local player fps, parameter is frame play interval, 0 as default (auto fps mode), others for fix fps mode, in milliseconds 
    int SetLocalVideoPlayInterval(int nInterval);

    //local player may call it when fast forward or backward
    void Reset();
    //end local player

    //Get associated camera session id, if no linked camera, return -1
    int GetCameraSession();

    //Get associated camera name
    const char *GetCameraName();

    //Set File saving parameters
    //If szFilePathName is NULL, will generate file name automatically
    int EnableFileSaving(bool bEnable, const char *szFilePathName = NULL);
    bool GetFileSavingState() { return m_bFileSaveEnable; };
    int SetFileSavingPath(const char *szFilePath);
    int SetFileNamePrefix(const char *szFilePrefix);
	int SetFileSaveType(int nType);    // 0 crv, 1 avi

    //Set jpeg auto saving (when capture image from device) 
    int EnableJPEGAutoSaving(bool bEnable);

    //Image Capture from video 
    int CaptureImage(const char *szFileName);

    //Text Overlap
    int EnableTextOverlap(bool bText, bool bTimeText);
    int SetOverlapTextContent(const char *pText);

    //Audio support (Speaker will recv audio data from camera and playback, Mic will send audio to camera)
    int Mic_Mute();
    int Mic_UnMute(int nType = 1);     //nType: 0 -- G723.1  1 -- AMR_NB, for 3G device, must be 1
    int Speaker_Mute();
    int Speaker_UnMute();
    bool GetMicState() { return m_bEnableAudioIn; };
    bool GetSpeakerState() { return m_bEnableAudioOut; };

    //if you set audio in from client as true, you CANNOT call Mic_UnMute() any more
    int SetAudioInFromClient(bool bAudioFromClient);
    //nType: 0 -- PCM (8k 16bit mono, 20ms)  1 -- AMR_NB
    int FeedAudioInDataFromClient(unsigned char *pAudioData, int nLen, int nType);   

    //End public interface
    /////////////////////////////////////////////////////////////////////////
    /////////////////////////////////////////////////////////////////////////
    //below is not for client use, they are for rtp callback and buffer callback
    //client MUST NOT call these functions
    //date source, from rtp
    void PutRTPStreamData(unsigned char *pBuffer,
                          int nBufferLen);

    //when RTP mutex has emptied this buffer, it would call this function
    void EmptyBufferDone(void* pBuffer);

    //receive event from RTP mutex
    void EventHandler(MP_ENG_EVENT event, void *pParam);
    
    //receive data from h.264 decode and saving
    void FileSaveHandler(void *pParam);
    
    //
    void SetCameraSession(int nCamSession);

protected:
    CPlaybackEngine(void);
    ~CPlaybackEngine();

    void SetIndex(int nIndex);

    void GenerateFileName(const char *type = "crv");

private:
    //parameters
    HWND m_hwnd;
    HDC m_hdc;
    int m_nVidWidth;
    int m_nVidHeight;
    int m_x;
    int m_y;
    int m_nDisplayWidth;
    int m_nDisplayHeight;
    int m_nLatencyTime;
    bool m_bDDraw;  //enable ddraw mode
    bool m_bVideoAutoScale;
    bool m_bEnableAudioOut;  //speaker
    bool m_bEnableAudioIn;   //mic
    bool m_bAudioInFromClient;   //
    void *m_pAMREncState;

    //engine state and it's index
    int m_state;
    int m_index; 

    int m_nCamSession;  //assosiated camera device
    char m_szCamName[64];

    MP_ENG_MODE m_mode; //working mode

    int m_nPlayInterval;

    //callback function for UI event, store ui app data to m_pAppData
    UIEventCallBack m_pCallbackFunc;
    void *m_pAppData;

    //For video file saving
    volatile bool m_bFileSaveEnable;
    volatile bool m_bCanFileSave;
    char m_szFilePrefix[64];
    char m_szFilePath[1024];
    char m_szFileName[1024];
	int  m_nFileSaveType;
    void *m_pAVIHandler;
    CRITICAL_SECTION m_fileLock;
    volatile bool m_bJPEGAutoSaving;

    //Text overlap
    bool m_bShowText;
    bool m_bShowTimeText;
    char m_szTextBuffer[64];
    
    //MM components
    CMemBufferPool *m_pMemBufferPool;
    RTPPacketMux *m_pRTPMux;
    H264Decode *m_pDecode;
    PostProcess *m_pPostProc;
    Render *m_pRender;
    G7231Decode *m_pAudioDecode;
    AudioRender *m_pAudioRender;
    
    //Start and stop network rtp service, should only called once
    static int StartNetService();
    static int StopNetService();

    friend class CCameraMngr;
};

#endif  //AFX_PLAYBACKENGINE_H__E213B48A_5511_4104_B474_A4C2779BB995__INCLUDED_
