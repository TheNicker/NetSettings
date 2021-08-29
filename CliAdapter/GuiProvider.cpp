
#include <string>
#include <functional>
#include <msclr\marshal.h>
#include <msclr\marshal_cppstd.h>
#include <msclr\marshal_windows.h>
#include "GuiProvider.h"

using namespace System;

ItemChangedCallbackType gCallBackToUnManaged = nullptr;

static void CallBackFromManaged(GuiProxy::GuiProvider::ItemChangedEventArgs item)
{
	using namespace msclr::interop;
	System::String^ k = item.key;
	System::String^ v = item.value;
	int m = (int)item.mode;
	

	ItemChangedArgs args;

	msclr::interop::marshal_context context;
	args.key = context.marshal_as<const wchar_t*>(k);
	args.val = context.marshal_as<const wchar_t*>(v);
	args.changedMode = static_cast<ItemChangedMode>(m);
	args.type = context.marshal_as<const wchar_t*>(item.type);
	
	gCallBackToUnManaged(&args);
}

void netsettings_GuiProviderTest()
{

	int k = 0;
}



void netsettings_Create(GuiCreateParams* createParams)
{
	
	GuiProxy::GuiProvider::CreateParams^ managedCreateParams = gcnew GuiProxy::GuiProvider::CreateParams();
	managedCreateParams->templateFilePath = gcnew String(createParams->templateFilePath);
	managedCreateParams->userSettingsFilePath = gcnew String(createParams->userSettingsFilePath);
	managedCreateParams->callback = gcnew  GuiProxy::GuiProvider::OnItemChangedDelegate(CallBackFromManaged);

	gCallBackToUnManaged = createParams->callback;

	GuiProxy::GuiProvider::Initialize(managedCreateParams);
}

void netsettings_SetVisible(bool visible)
{
	GuiProxy::GuiProvider::SetVisible(visible);
}




