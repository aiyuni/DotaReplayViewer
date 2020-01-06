TestSend()
{
	Send, -
	Sleep, 2000
	Click, X1
	sleep, 2000
	Click, X2
	Sleep, 2000
	return
}

WatchThisReplay(id, playerSlot)
{
	Send playdemo
	Sleep, 1000
	Send {Space}
	Sleep, 2000
	SendRaw "C:/Program Files (x86)/Steam/steamapps/common/dota 2 beta/game/dota/replays/%id%.dem"  ;C:\Program Files (x86)\Steam\steamapps\common\dota 2 beta\game\dota\replays
	Sleep, 2000
	Send {Enter}
	Sleep, 15000
	Loop, %playerSlot%
	{
		SendRaw dota_spectator_selectnexthero
		Sleep, 300
		Send {Enter}
		Sleep, 300
	}
	SendRaw dota_spectator_mode 3
	Sleep, 1000
	Send {Enter}
	Click, X2
	return
}

StartStream()
{
	Send ^{[}
	return
}

StopStream()
{
	Send ^{]}
	return
}
