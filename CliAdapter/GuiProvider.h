#pragma once
#include "Platform.h"
#include <string>
#include <functional>

#ifdef GUI_PROVIDER_LIB
#define GUI_PROVIDER_API LLUTILS_EXPORT  
#else
#define GUI_PROVIDER_API  LLUTILS_IMPORT 
#endif



enum class ItemChangedMode
{
	  None
	, OnTheFly
	, UserConfirmed
	, Synthesized
};

struct ItemChangedArgs
{
	const wchar_t* key;
	const wchar_t* val;
	const wchar_t* type;
	ItemChangedMode changedMode;
};

//using ItemChangedCallbackType = (void*)(ItemChangedArgs*);
using ItemChangedCallbackType = void (*)(ItemChangedArgs*);




struct GuiCreateParams
{
	const wchar_t* templateFilePath;
	const wchar_t* userSettingsFilePath;
	ItemChangedCallbackType callback;

};



 //using CallBackType = std::function<void(ItemChangedArgs&)>;

 extern "C"
 {
	 GUI_PROVIDER_API void netsettings_Create(GuiCreateParams*);
	 GUI_PROVIDER_API void netsettings_SetVisible(bool visible);
	 GUI_PROVIDER_API void netsettings_GuiProviderTest();
 }

 /*class GUI_PROVIDER_API GUIProvider
 {
 public:
	 static void Create(std::wstring filePath, CallBackType managedCallback);
	 static void SetVisible(bool visible);
	 
 };*/



