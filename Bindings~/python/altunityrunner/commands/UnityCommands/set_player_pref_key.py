from altunityrunner.commands.base_command import BaseCommand
from altunityrunner.player_pref_key_type import PlayerPrefKeyType
from loguru import logger
class SetPlayerPrefKey(BaseCommand):
    def __init__(self, socket,request_separator,request_end,key_name, value, key_type):
        super(SetPlayerPrefKey, self).__init__(socket,request_separator,request_end)
        self.key_name=key_name
        self.value=value
        self.key_type=key_type
    
    def execute(self):
        data = ''
        if self.key_type is 1:
            logger.trace('Set Int Player Pref for key: ' + self.key_name + ' to ' + str(self.value))
            data = self.send_data(self.create_command('setKeyPlayerPref', self.key_name , str(self.value) , str(PlayerPrefKeyType.Int) ))
        if self.key_type is 2:
            logger.trace('Set String Player Pref for key: ' + self.key_name + ' to ' + str(self.value))
            data = self.send_data(self.create_command('setKeyPlayerPref', self.key_name , str(self.value) , str(PlayerPrefKeyType.String) ))
        if self.key_type is 3:
            logger.trace('Set Float Player Pref for key: ' + self.key_name + ' to ' + str(self.value))
            data = self.send_data(self.create_command('setKeyPlayerPref', self.key_name , str(self.value) , str(PlayerPrefKeyType.Float) ))
        return self.handle_errors(data)