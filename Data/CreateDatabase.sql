CREATE DATABASE
            [Test]
        ON PRIMARY (
           NAME=Test_data,
           FILENAME = 'C:\Users\jypjy\source\repos\LoveDotNet.Blazor\Data\lovedotnet.mdf'
        )
        LOG ON (
            NAME=Test_log,
            FILENAME = 'C:\Users\jypjy\source\repos\LoveDotNet.Blazor\Data\lovedotnet_log.ldf'
        )