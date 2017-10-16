// The following ifdef block is the standard way of creating macros which make exporting 
// from a DLL simpler. All files within this DLL are compiled with the CONTROLCOMMAND_EXPORTS
// symbol defined on the command line. this symbol should not be defined on any project
// that uses this DLL. This way any other project whose source files include this file see 
// CONTROLCOMMAND_API functions as being imported from a DLL, whereas this DLL sees symbols
// defined with this macro as being exported.
#ifdef CONTROLCOMMAND_EXPORTS
#define CONTROLCOMMAND_API __declspec(dllexport)
#else
#define CONTROLCOMMAND_API __declspec(dllimport)
#endif

enum VideoSize
{
	Big = 0,
	Normal = 1,
	Small = 2
};

enum ReturnValue
{
	OK,
	ErrorCommand,
	ErrorParam,
	NotImplemented,
	ExecuteError,
	NoPermission,
	IdOK,
	AuthorizationOK,
	ChannelNotFound,
	IdNotFound,
	AuthorizationFailed
};

typedef struct _Return
{
	int sequence;
	ReturnValue value;
}
Return;


// This class is exported from the ControlCommand.dll
class CONTROLCOMMAND_API ControlCommandHelper {
public:
	ControlCommandHelper(void);
	
	/** Generates a text command that tells the camera to go up, which can be used 
	 * in the CCameraMngr::SendControlCmd method.
	 * @param camChannel The session id of the camera to which to send the command.
	 * @param speed The motion speed, must be in range 0 - 100.
	 * @param commandBuffer The pointer to the buffer which receives the command.
	 * @return The sequence number of the generated command, which you use to 
	 * identify which command the return info corresponds to when you receive 
	 * one from the camera, or -1 if some error occurred.
	 */
	int GenerateGoUpCommand(int camChannel, int speed, char* commandBuffer);

	int GenerateGoDownCommand(int camChannel, int speed, char* commandBuffer);

	int GenerateGoLeftCommand(int camChannel, int speed, char* commandBuffer);

	int GenerateGoRightCommand(int camChannel, int speed, char* commandBuffer);

	int GenerateZoomWideCommand(int camChannel, int speed, char* commandBuffer);

	int GenerateZoomTeleCommand(int camChannel, int speed, char* commandBuffer);

	int GenerateFocusNearCommand(int camChannel, int speed, char* commandBuffer);

	int GenerateFocusFarCommand(int camChannel, int speed, char* commandBuffer);

	int GenerateIrisOpenCommand(int camChannel, int speed, char* commandBuffer);

	int GenerateIrisCloseCommand(int camChannel, int speed, char* commandBuffer);

	int GenerateAutoScanCommand(int camChannel, bool enable, char* commandBuffer);

	int GenerateCruiseCommand(int camChannel, bool enable, char* commandBuffer);

	int GenerateInfraredCommand(int camChannel, bool enable, char* commandBuffer);

	int GenerateRainStripCommand(int camChannel, bool enable, char* commandBuffer);

	int GeneratePresetCommand(int camChannel, int presetNumber, char* commandBuffer);

	int GenerateWarningOutputCommand(int camChannel, int port, bool enable, char* commandBuffer);

	int GenerateSetVideoParamCommand(int camChannel, VideoSize size, int maxFPS, int quality, char* commandBuffer);

	int GenerateRebootCommand(int camChannel, char* commandBuffer);
	
	/** Parse the returned string into an array of the Return type. The returned string
	 * is received in pParam parameter when the event parameter is MP_EVENT_ACK, 
	 * in the UIEventCallBack event handler functions.
	 * @param string The returned string to parse.
	 * @param buffer The starting address of the buffer which receives the parsing results.
	 * The results are stored into the buffer as an array of the Return type.
	 * You should allocate adequate space because multiple Return objects could be parsed.
	 * Generally, quintuple of the size of the Return type is adequate in most cases.
	 * @param sizeofBuffer The size of the buffer, in bytes.
	 * @return The count of the parsed Return objects. If the parsing failed, zero is returned.
	 */
	int ParseReturnString(const char* string, Return* buffer, int sizeofBuffer);

private:
	int GenerateActionCommand(int camChannel, const char* action, int speed, char* commandBuffer);

	int GenerateEnableCommand(int camChannel, const char* item, bool enable, char* commandBuffer);
};
