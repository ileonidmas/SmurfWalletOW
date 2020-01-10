#pragma once
class HookListener
{

public:
	HookListener();
	~HookListener();

	typedef void(__stdcall* HookCallback)();
	HookCallback hookCallback = nullptr;
	bool Hook(int* handle);
	bool Unhook(void);
};

