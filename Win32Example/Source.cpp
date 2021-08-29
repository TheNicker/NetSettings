#include <Windows.h>
#include <sstream>
#include <iostream>

#include "../CliAdapter/GuiProvider.h"

static void MessageLoop()
{
    MSG msg;
    BOOL bRet;
    while ((bRet = GetMessage(&msg, nullptr, 0, 0)) != 0)
    {
        if (bRet == -1)
        {
            // handle the error and possibly exit
        }
        else
        {
            TranslateMessage(&msg);
            DispatchMessage(&msg);
        }
    }
}
void OnItemChanged(ItemChangedArgs* argsPtr)
{
    const auto& args = *argsPtr;
    auto GetChangedMode = [&]()
    {
        switch (args.changedMode)
        {
        case ItemChangedMode::OnTheFly:
            return "On the fly";
        case ItemChangedMode::Synthesized:
            return "Synthesized";
        case ItemChangedMode::UserConfirmed:
            return "User confirmed";
        case ItemChangedMode::None:
            return "undefined";
        default:
            return "error";
        }
    };
    std::wstringstream ss;
    ss << L"[changed setting] name: " << args.key << L" | value: " << args.val << L" | type: " << args.type <<  L" | mode: " << GetChangedMode() << std::endl;

    std::wcout << ss.str();
    int k = 0;
}

template<typename T, typename... U>
void* getAddress(std::function<T(U...)> f) {
    typedef T(fnType)(U...);
    fnType** fnPointer = f.template target<fnType*>();
    return fnPointer != nullptr ? reinterpret_cast<void*>(*fnPointer) : nullptr;
}

int main()
{
    std::wstring templateFilePath = L"f:/Development/NetSettings/x64/Debug/Resources/GuiTemplate.json";
    std::wstring userSettingsFilePath = L"f:/Development/NetSettings/x64/Debug/Resources/userSettings.json";
    
    GuiCreateParams params;
    
    params.callback = &OnItemChanged;
    params.templateFilePath = templateFilePath.c_str();
    params.userSettingsFilePath = userSettingsFilePath.c_str();


    netsettings_Create(&params);
    netsettings_SetVisible(true);
    
    MessageLoop();
}


//int WINAPI wWinMain(HINSTANCE hInstance, HINSTANCE hPrevInstance, PWSTR pCmdLine, int nCmdShow)
//
//{
//    
//   
//}




