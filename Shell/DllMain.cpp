
#include "DllMain.h"
#include "HookListener.h"

#include <windows.h>
#include <iostream>
#include <fstream>

using namespace std;


extern "C" {
    typedef void(__stdcall* NotificationCallback)();
    NotificationCallback notificationCallback = nullptr;
    __declspec(dllexport) void  __cdecl SetNotificationCallback(NotificationCallback callback) {
        notificationCallback = callback;
    }

    __declspec(dllexport) LRESULT CALLBACK CallWndProc(int nCode, WPARAM wParam, LPARAM lParam) {

        if (nCode < 0) {
            return CallNextHookEx(NULL, nCode, wParam, lParam);
        }

        if (nCode == HSHELL_REDRAW) {
            ofstream file;
            file.open("C:\\Users\\lema\\Desktop\\scripts\\Test.txt", ios_base::app);
            file << "Redraw was called\n"  << " \n";
            file.close();
        }
        ofstream file;
        file.open("C:\\Users\\lema\\Desktop\\scripts\\Test.txt", ios_base::app);

        if (notificationCallback != nullptr) {
            notificationCallback();
            file << "NCode: " << nCode << " wParam: " << wParam << " lParam: " << lParam << " pointer: " << notificationCallback << " \n";

        }
        else {
            file << "NCode: " << nCode << " wParam: " << wParam << " lParam: " << lParam << " pointer: " << " null \n";

        }
        file.close();
        
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