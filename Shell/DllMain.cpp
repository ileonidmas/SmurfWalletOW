
#include "DllMain.h"
#include "HookListener.h"

#include <windows.h>
#include <iostream>
#include <fstream>

//these variables will be shared among all processes to which this dll is linked
#pragma data_seg("Shared")
HWND _mainHwnd = nullptr;
#pragma data_seg() //end of our data segment

#pragma comment(linker,"/section:Shared,rws")
// Tell the compiler that Shared section can be read,write and shared
using namespace std;

extern "C" {
    

    __declspec(dllexport) void  __cdecl SetWindowHandle(HWND mainHwnd) {
        _mainHwnd = mainHwnd;
    }

    __declspec(dllexport) LRESULT CALLBACK CallWndProc(int nCode, WPARAM wParam, LPARAM lParam) {

        if (nCode < 0) {
            return CallNextHookEx(NULL, nCode, wParam, lParam);
        }

        if (nCode == HSHELL_REDRAW) {
            SendMessage(_mainHwnd, 0X400, 1, 0); // notify user that something happened
        }              
        return CallNextHookEx(NULL, nCode, wParam, lParam);
    }
}

BOOL APIENTRY DllMain(HMODULE hDLL, DWORD Reason, LPVOID Reserved) {

    switch (Reason) {
    case DLL_PROCESS_ATTACH: break;
    case DLL_PROCESS_DETACH: break;
    case DLL_THREAD_ATTACH:  break;
    case DLL_THREAD_DETACH:  break;
    }

    return TRUE;
}