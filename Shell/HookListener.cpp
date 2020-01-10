#include "HookListener.h"
#include <windows.h>
#include <tchar.h>
#include <windows.h>
#include <stdio.h>
#include <psapi.h>
#pragma comment( lib, "user32.lib") 
#pragma comment( lib, "gdi32.lib")

HookListener::HookListener()
{
}


HookListener::~HookListener()
{
}

HHOOK shell_hook;


bool HookListener::Hook(int* handle) {

    auto thread_id = GetWindowThreadProcessId((HWND)handle, NULL);
    auto dll = LoadLibrary(TEXT("C:\\Users\\lema\\Documents\\Github\\SmurfWalletOW\\SmurfWalletOW\\bin\\Debug\\Shell.dll"));
    auto address = (HOOKPROC)GetProcAddress(dll, "CallWndProc");
    shell_hook = SetWindowsHookEx(WH_SHELL, address, dll, thread_id); 

	return true;
}


bool HookListener::Unhook(void) {
	UnhookWindowsHookEx(shell_hook);
	return true;
}