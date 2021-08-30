#include <Windows.h>
#include <sstream>
#include <iostream>
#include <filesystem>

#include "../CliAdapter/GuiProvider.h"
#include "../CliAdapter/Platform.h"

static void MessageLoop()
{
    MSG msg;
    BOOL bRet;
    while ((bRet = GetMessage(&msg, nullptr, 0, 0)) != 0)
    {
        if (bRet == -1)
        {
            exit(-1);
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

BOOL WINAPI CtrlHandler(DWORD dwType)
{
    PostQuitMessage(0);
    return TRUE;
}

int main()
{
    wchar_t ownPth[MAX_PATH];
    GetModuleFileName(GetModuleHandle(nullptr), ownPth, (sizeof(ownPth) / sizeof(ownPth[0])));
    auto folderPath = std::filesystem::path(ownPth).remove_filename();
    auto templatePath = folderPath / L"Resources/GuiTemplate.json";
    auto userSettingsPath = folderPath / L"userSettings.json";

    
    std::wstring templateFilePath = templatePath.wstring();
    std::wstring userSettingsFilePath = userSettingsPath.wstring();
    
    GuiCreateParams params;
    
    params.callback = &OnItemChanged;
    params.templateFilePath = templateFilePath.c_str();
    params.userSettingsFilePath = userSettingsFilePath.c_str();


    netsettings_Create(&params);
    netsettings_SetVisible(true);
    
    SetConsoleCtrlHandler(CtrlHandler, TRUE);

    MessageLoop();
}



