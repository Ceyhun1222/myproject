import { _server } from "./_dev";

export const _config = {
  links: {
    login: _server.host + '/api/OAuth/Login',
    logout: _server.host + '/api/OAuth/LogOut',
    signup: _server.host + '/api/OAuth/SignUp',
    user: _server.host + '/api/Users',
    userBasicInfo: _server.host + '/api/Users/BasicInfos',
    profile: _server.host + '/api/Users/Profile',
    userNotificationCount: _server.host + '/api/Users/NotificationCount',
    checkUserName: _server.host + '/api/OAuth/CheckUsername?username=',
    checkEmail: _server.host + '/api/OAuth/CheckEmail?email=',
    refresh: _server.host + '/api/OAuth/Refresh',
    forgotPassword: _server.host + '/api/OAuth/ForgotPassword',
    resetPassword: _server.host + '/api/OAuth/ResetPassword',
    slotList: _server.host + '/api/Slots',
    slotSave: _server.host + '/api/Slots',
    slotSelected: _server.host + '/api/Slots/Selected',
    airports: _server.host + '/api/Data/Airports',
    airportsGeo: _server.host + '/api/Data/Airports/Geojson',
    request: _server.host + '/api/Request',
    requestMine: _server.host + '/api/Request/Mine',
    requestSubmit: _server.host + '/api/Request/Submit/',
    reportTree: _server.host + '/api/Request/ReportTree?id=',
    requestNotificationCount: _server.host + '/api/Request/NotificationCount',
    requestCheck: _server.host + '/api/Request/Check/',
    verticalStructuresGeo: _server.host + '/api/Data/VerticalStructures/',
    obstacleAreasGeo: _server.host + '/api/Data/ObstacleAreas/',
    requestGeo: _server.host + '/api/Request/GeoJson/',
    reportData: _server.host + '/api/Request/ReportData/',
    verticalStructuresDto: _server.host +  "/api/Data/VerticalStructuresDto",
    submit2Aixm: _server.host +  "/api/Request/Submit2Aixm/",
    getAttachment: _server.host + '/api/Request/Attachment/',
    downloadReport: _server.host + '/api/Request/DownloadReport/'
  },
  phone: {
    prefix: "+7 ",
    mask: "(0000) 000 000"
  },
  verticalStructuresDtoList: [
    {
      "identifier": "60f66026-3b8e-4357-8fc4-68b2241c2f9b",
      "name": "AA_G_1",
      "type": null,
      "elevation": 853.03,
      "verticalAccuracy": 0.101,
      "horizontalAccuracy": 0.188,
      "geo": {
        "type": "Point",
        "coordinates": [
          76.943168416,
          43.260987405,
          0
        ]
      }
    },
    {
      "identifier": "60f66026-3b8e-4357-8fc4-68b2241c2f9b",
      "name": "AA_G_1",
      "type": null,
      "elevation": 855.53,
      "verticalAccuracy": 0.101,
      "horizontalAccuracy": 0.188,
      "geo": {
        "type": "Point",
        "coordinates": [
          76.943536436,
          43.261079893,
          0
        ]
      }
    },
    {
      "identifier": "aa00c3f5-bea1-4b15-8c92-48f2cb58402d",
      "name": "AA_G_2",
      "type": null,
      "elevation": 829.48,
      "verticalAccuracy": 0.101,
      "horizontalAccuracy": 0.188,
      "geo": {
        "type": "Point",
        "coordinates": [
          76.938910762,
          43.2580151370001,
          0
        ]
      }
    },
    {
      "identifier": "78496a35-1288-476d-a7fd-9b717946c262",
      "name": "AA_G_4",
      "type": null,
      "elevation": 828.43,
      "verticalAccuracy": 0.101,
      "horizontalAccuracy": 0.188,
      "geo": {
        "type": "Point",
        "coordinates": [
          76.9558553190001,
          43.2619393000001,
          0
        ]
      }
    },
    {
      "identifier": "8cde9774-4503-4cdc-85c9-68fcdb06b734",
      "name": "AA0003OB0102",
      "type": "TREE",
      "elevation": 682.155,
      "verticalAccuracy": 0.037,
      "horizontalAccuracy": 0.039,
      "geo": {
        "type": "Point",
        "coordinates": [
          77.0730272075,
          43.3688847030556,
          0
        ]
      }
    },
    {
      "identifier": "698f9d3b-ca55-4f30-81ab-1f9a2dd21185",
      "name": "AA0006OB0201U",
      "type": "STACK",
      "elevation": 729,
      "verticalAccuracy": 0,
      "horizontalAccuracy": 0,
      "geo": {
        "type": "Point",
        "coordinates": [
          77.0077777777778,
          43.4241666666667,
          0
        ]
      }
    },
    {
      "identifier": "b91c254d-ab11-47fc-82ff-9b85083e3e3e",
      "name": "AA0009OB0101",
      "type": "ANTENNA",
      "elevation": 686.379,
      "verticalAccuracy": 0.037,
      "horizontalAccuracy": 0.039,
      "geo": {
        "type": "Point",
        "coordinates": [
          77.0732526172222,
          43.36879567,
          0
        ]
      }
    },
    {
      "identifier": "7f745d4e-6e47-4ede-962c-8909bd3acf71",
      "name": "AA0011OB0102",
      "type": "ANTENNA",
      "elevation": 679.656,
      "verticalAccuracy": 0.037,
      "horizontalAccuracy": 0.039,
      "geo": {
        "type": "Point",
        "coordinates": [
          77.0717978075,
          43.3681653347222,
          0
        ]
      }
    },
    {
      "identifier": "8fbb44f7-8351-4083-9478-1b69ab7f520d",
      "name": "AA0012OB0101",
      "type": "ANTENNA",
      "elevation": 687.784,
      "verticalAccuracy": 0.037,
      "horizontalAccuracy": 0.039,
      "geo": {
        "type": "Point",
        "coordinates": [
          77.0732608847222,
          43.3687612052778,
          0
        ]
      }
    },
    {
      "identifier": "ca7e6466-7f88-43a4-b643-6b2f4931f1ec",
      "name": "AA0019OB0102",
      "type": null,
      "elevation": 686.496,
      "verticalAccuracy": 0.037,
      "horizontalAccuracy": 0.039,
      "geo": {
        "type": "Point",
        "coordinates": [
          77.0101228833333,
          43.3384312541667,
          0
        ]
      }
    },
    {
      "identifier": "89461924-a57f-4005-8e42-fb8df59ac3c0",
      "name": "AA0020OB0102",
      "type": null,
      "elevation": 681.929,
      "verticalAccuracy": 0.037,
      "horizontalAccuracy": 0.039,
      "geo": {
        "type": "Point",
        "coordinates": [
          77.0115963811111,
          43.3383391747222,
          0
        ]
      }
    },
    {
      "identifier": "9d6d6a93-4469-4436-b75e-6ad653045dc9",
      "name": "AA0025OB0102",
      "type": null,
      "elevation": 677.738,
      "verticalAccuracy": 0.037,
      "horizontalAccuracy": 0.039,
      "geo": {
        "type": "Point",
        "coordinates": [
          77.0176322558333,
          43.3438961638889,
          0
        ]
      }
    },
    {
      "identifier": "f28a3e4a-3482-484c-a1a8-40525d78f74f",
      "name": "AA0026OB0102",
      "type": null,
      "elevation": 677.489,
      "verticalAccuracy": 0.037,
      "horizontalAccuracy": 0.039,
      "geo": {
        "type": "Point",
        "coordinates": [
          77.0175250605556,
          43.3438418872222,
          0
        ]
      }
    },
    {
      "identifier": "d8488fbb-48f9-43dc-b62a-e300b113107d",
      "name": "AA0027OB0102",
      "type": null,
      "elevation": 682.197,
      "verticalAccuracy": 0.037,
      "horizontalAccuracy": 0.039,
      "geo": {
        "type": "Point",
        "coordinates": [
          77.0179345961111,
          43.3437521811111,
          0
        ]
      }
    },
    {
      "identifier": "95bbb0e5-6c00-4ea2-875d-5bbf5734bf8f",
      "name": "AA0028OB0102",
      "type": null,
      "elevation": 677.915,
      "verticalAccuracy": 0.037,
      "horizontalAccuracy": 0.039,
      "geo": {
        "type": "Point",
        "coordinates": [
          77.0192143822222,
          43.3443860913889,
          0
        ]
      }
    },
    {
      "identifier": "a21c03a9-9514-47ca-b0a0-6af43f47c317",
      "name": "AA0029OB0103",
      "type": "ANTENNA",
      "elevation": 692.099,
      "verticalAccuracy": 0.037,
      "horizontalAccuracy": 0.039,
      "geo": {
        "type": "Point",
        "coordinates": [
          77.0671150147222,
          43.3696346833333,
          0
        ]
      }
    },
    {
      "identifier": "58eb7b81-40fe-4186-8302-03debb7c1b9a",
      "name": "AA0032OB0104",
      "type": "ANTENNA",
      "elevation": 687.225,
      "verticalAccuracy": 0.037,
      "horizontalAccuracy": 0.039,
      "geo": {
        "type": "Point",
        "coordinates": [
          77.0668460097222,
          43.3695111208333,
          0
        ]
      }
    },
    {
      "identifier": "6fded728-1cd8-4979-ab10-b5f7da4b3d8f",
      "name": "AA0033OB0104",
      "type": "ANTENNA",
      "elevation": 687.077,
      "verticalAccuracy": 0.037,
      "horizontalAccuracy": 0.039,
      "geo": {
        "type": "Point",
        "coordinates": [
          77.0667351055556,
          43.3693960805556,
          0
        ]
      }
    },
    {
      "identifier": "eb8ba184-8a1e-4298-9f4f-b42afe735f3d",
      "name": "AA0034OB0102",
      "type": "ANTENNA",
      "elevation": 682.103,
      "verticalAccuracy": 0.037,
      "horizontalAccuracy": 0.039,
      "geo": {
        "type": "Point",
        "coordinates": [
          77.0667038491667,
          43.3692156980556,
          0
        ]
      }
    },
    {
      "identifier": "fb1130ad-9e24-48ff-9343-61f831ae2efa",
      "name": "AA0035OB0102",
      "type": "ANTENNA",
      "elevation": 681.864,
      "verticalAccuracy": 0.037,
      "horizontalAccuracy": 0.039,
      "geo": {
        "type": "Point",
        "coordinates": [
          77.0667936280555,
          43.3691281483333,
          0
        ]
      }
    },
    {
      "identifier": "e68a2609-5c7e-4a39-94ec-82507161a25a",
      "name": "AA0036OB0102",
      "type": "ANTENNA",
      "elevation": 682.083,
      "verticalAccuracy": 0.037,
      "horizontalAccuracy": 0.039,
      "geo": {
        "type": "Point",
        "coordinates": [
          77.0670165525,
          43.3693711019444,
          0
        ]
      }
    },
    {
      "identifier": "7ef6dca8-26b8-42c8-9a9b-689ac9244c7d",
      "name": "AA0037OB0102",
      "type": "ANTENNA",
      "elevation": 681.739,
      "verticalAccuracy": 0.037,
      "horizontalAccuracy": 0.039,
      "geo": {
        "type": "Point",
        "coordinates": [
          77.0671018202778,
          43.3692862808333,
          0
        ]
      }
    },
    {
      "identifier": "ac00faf2-fe29-46a8-9467-9f7509c61c76",
      "name": "AA0038OB0102",
      "type": "ANTENNA",
      "elevation": 684.873,
      "verticalAccuracy": 0.037,
      "horizontalAccuracy": 0.039,
      "geo": {
        "type": "Point",
        "coordinates": [
          77.0671836977778,
          43.3694304425,
          0
        ]
      }
    },
    {
      "identifier": "5b00d835-af7e-44da-a7b8-b58e327d1db4",
      "name": "AA0039OB0102",
      "type": "ANTENNA",
      "elevation": 682.728,
      "verticalAccuracy": 0.037,
      "horizontalAccuracy": 0.039,
      "geo": {
        "type": "Point",
        "coordinates": [
          77.0677369622222,
          43.3699502419444,
          0
        ]
      }
    },
    {
      "identifier": "928ac6df-8e2c-4f5b-b749-2aa2b13c2051",
      "name": "AA0040OB0102",
      "type": "TOWER",
      "elevation": 682.839,
      "verticalAccuracy": 0.037,
      "horizontalAccuracy": 0.039,
      "geo": {
        "type": "Point",
        "coordinates": [
          77.0664737375,
          43.3709181691667,
          0
        ]
      }
    },
    {
      "identifier": "f0b88c18-16bb-40fb-b506-6da5ed4c2ddd",
      "name": "AA0041OB0104",
      "type": "ELECTRICAL_SYSTEM",
      "elevation": 695.029,
      "verticalAccuracy": 0.037,
      "horizontalAccuracy": 0.039,
      "geo": {
        "type": "Point",
        "coordinates": [
          77.02155527,
          43.3418334658333,
          0
        ]
      }
    },
    {
      "identifier": "5e9ca8ef-9a7d-48aa-9efa-eabe47982cc0",
      "name": "AA0044OB0102",
      "type": "ANTENNA",
      "elevation": 684.217,
      "verticalAccuracy": 0.037,
      "horizontalAccuracy": 0.039,
      "geo": {
        "type": "Point",
        "coordinates": [
          77.0223915355556,
          43.3419586913889,
          0
        ]
      }
    },
    {
      "identifier": "76854d37-369c-4a1e-a0d1-d9950ee6c66f",
      "name": "AA0045OB0102",
      "type": "POLE",
      "elevation": 681.436,
      "verticalAccuracy": 0.037,
      "horizontalAccuracy": 0.039,
      "geo": {
        "type": "Point",
        "coordinates": [
          77.0224271036111,
          43.3425226627778,
          0
        ]
      }
    },
    {
      "identifier": "e739c968-2beb-47cf-9f27-c243f5b1d4b5",
      "name": "AA0046OB0102",
      "type": "POLE",
      "elevation": 681.433,
      "verticalAccuracy": 0.037,
      "horizontalAccuracy": 0.039,
      "geo": {
        "type": "Point",
        "coordinates": [
          77.0228144255556,
          43.3428299136111,
          0
        ]
      }
    },
    {
      "identifier": "ffa3ae97-f64f-435f-90a0-e61eea9aca91",
      "name": "AA0047OB0102",
      "type": "POLE",
      "elevation": 681.421,
      "verticalAccuracy": 0.037,
      "horizontalAccuracy": 0.039,
      "geo": {
        "type": "Point",
        "coordinates": [
          77.0231207358333,
          43.3429831119444,
          0
        ]
      }
    },
    {
      "identifier": "4969418a-efe1-4625-aa9d-3c8cccd079eb",
      "name": "AA0048OB0102",
      "type": "TREE",
      "elevation": 681.443,
      "verticalAccuracy": 0.037,
      "horizontalAccuracy": 0.039,
      "geo": {
        "type": "Point",
        "coordinates": [
          77.0231884758333,
          43.3429106086111,
          0
        ]
      }
    },
    {
      "identifier": "7eaa8c75-1b2c-4950-9492-32f31165923b",
      "name": "AA0052OB0104",
      "type": "ANTENNA",
      "elevation": 684.112,
      "verticalAccuracy": 0.037,
      "horizontalAccuracy": 0.039,
      "geo": {
        "type": "Point",
        "coordinates": [
          77.0206157544444,
          43.341368925,
          0
        ]
      }
    },
    {
      "identifier": "b196033c-fb3b-4190-a080-09f58558e0d1",
      "name": "AA0054OB0102",
      "type": null,
      "elevation": 680.498,
      "verticalAccuracy": 0.037,
      "horizontalAccuracy": 0.039,
      "geo": {
        "type": "Point",
        "coordinates": [
          77.0265054833333,
          43.3493118061111,
          0
        ]
      }
    },
    {
      "identifier": "67aeb930-6f99-49cc-8682-fdadd6e30ec8",
      "name": "AA0055OB0103",
      "type": "ANTENNA",
      "elevation": 688.993,
      "verticalAccuracy": 0.037,
      "horizontalAccuracy": 0.039,
      "geo": {
        "type": "Point",
        "coordinates": [
          77.0274101525,
          43.3497548730556,
          0
        ]
      }
    },
    {
      "identifier": "8175fd7c-010e-4748-afb6-3f2e61bde336",
      "name": "AA0057OB0102",
      "type": null,
      "elevation": 682.021,
      "verticalAccuracy": 0.037,
      "horizontalAccuracy": 0.039,
      "geo": {
        "type": "Point",
        "coordinates": [
          77.0277321897222,
          43.3498509452778,
          0
        ]
      }
    },
    {
      "identifier": "92b7d429-9905-4645-8fe2-8700a2466e11",
      "name": "AA0059OB0102",
      "type": null,
      "elevation": 679.29,
      "verticalAccuracy": 0.037,
      "horizontalAccuracy": 0.039,
      "geo": {
        "type": "Point",
        "coordinates": [
          77.0280961166667,
          43.3500447883333,
          0
        ]
      }
    },
    {
      "identifier": "09c29fe4-bce8-456b-8aac-bef5d139f6cf",
      "name": "AA0060OB0102",
      "type": null,
      "elevation": 679.435,
      "verticalAccuracy": 0.037,
      "horizontalAccuracy": 0.039,
      "geo": {
        "type": "Point",
        "coordinates": [
          77.02843568,
          43.3501694602778,
          0
        ]
      }
    },
    {
      "identifier": "95d86bcd-4133-4a6b-a14c-c36aa9c9c704",
      "name": "AA0061OB0102",
      "type": null,
      "elevation": 678.992,
      "verticalAccuracy": 0.037,
      "horizontalAccuracy": 0.039,
      "geo": {
        "type": "Point",
        "coordinates": [
          77.0285042280556,
          43.3500915461111,
          0
        ]
      }
    },
    {
      "identifier": "edd9b1fa-7b88-4a25-841c-2beb9e28735c",
      "name": "AA0062OB0102",
      "type": null,
      "elevation": 678.987,
      "verticalAccuracy": 0.037,
      "horizontalAccuracy": 0.039,
      "geo": {
        "type": "Point",
        "coordinates": [
          77.0281543733333,
          43.3499609072222,
          0
        ]
      }
    },
    {
      "identifier": "d40fd15a-f12d-41f2-9c3f-a285b3780f9f",
      "name": "AA0063OB0104",
      "type": null,
      "elevation": 684.59,
      "verticalAccuracy": 0.037,
      "horizontalAccuracy": 0.039,
      "geo": {
        "type": "Point",
        "coordinates": [
          77.0280432686111,
          43.3501362183333,
          0
        ]
      }
    },
    {
      "identifier": "4278a06d-c288-43cf-a8aa-620f07c15d41",
      "name": "AA0064OB0104",
      "type": null,
      "elevation": 684.74,
      "verticalAccuracy": 0.037,
      "horizontalAccuracy": 0.039,
      "geo": {
        "type": "Point",
        "coordinates": [
          77.0284407933333,
          43.350268875,
          0
        ]
      }
    },
    {
      "identifier": "59dc2b17-d10f-4f89-8c4c-1ea9976388f3",
      "name": "AA0067OB0121",
      "type": "BUILDING",
      "elevation": 703.49,
      "verticalAccuracy": 0.037,
      "horizontalAccuracy": 0.039,
      "geo": {
        "type": "Point",
        "coordinates": [
          77.0122842572222,
          43.3463545388889,
          0
        ]
      }
    },
    {
      "identifier": "18bf4458-592a-4f44-9ed5-5b69dacc5add",
      "name": "AA0068OB0101",
      "type": "POLE",
      "elevation": 700.217,
      "verticalAccuracy": 0.037,
      "horizontalAccuracy": 0.039,
      "geo": {
        "type": "Point",
        "coordinates": [
          77.0131491627778,
          43.3462005302778,
          0
        ]
      }
    },
    {
      "identifier": "e09e91c4-f77a-4ef5-94ba-4c8a23c7e967",
      "name": "AA0069OB0101",
      "type": "ELECTRICAL_EXIT_LIGHT",
      "elevation": 699.99,
      "verticalAccuracy": 0.037,
      "horizontalAccuracy": 0.039,
      "geo": {
        "type": "Point",
        "coordinates": [
          77.0128789997222,
          43.3466307633333,
          0
        ]
      }
    },
    {
      "identifier": "6252212e-2f1f-4a36-a0a0-97dee2b07569",
      "name": "AA0070OB0101",
      "type": "ELECTRICAL_EXIT_LIGHT",
      "elevation": 699.706,
      "verticalAccuracy": 0.037,
      "horizontalAccuracy": 0.039,
      "geo": {
        "type": "Point",
        "coordinates": [
          77.0125907191667,
          43.3470952786111,
          0
        ]
      }
    },
    {
      "identifier": "0b9f0026-99fd-4acb-b2af-b7e8ad2f8038",
      "name": "AA0071OB0101",
      "type": "ELECTRICAL_EXIT_LIGHT",
      "elevation": 698.912,
      "verticalAccuracy": 0.037,
      "horizontalAccuracy": 0.039,
      "geo": {
        "type": "Point",
        "coordinates": [
          77.0123195213889,
          43.3475254202778,
          0
        ]
      }
    },
    {
      "identifier": "0c775ac5-a53e-4134-a5f2-f87f213404e8",
      "name": "AA0072OB0101",
      "type": "ELECTRICAL_EXIT_LIGHT",
      "elevation": 698.168,
      "verticalAccuracy": 0.037,
      "horizontalAccuracy": 0.039,
      "geo": {
        "type": "Point",
        "coordinates": [
          77.0120262797222,
          43.3478893361111,
          0
        ]
      }
    },
    {
      "identifier": "07f32ed9-f036-4387-9803-5bbf5cd0ab00",
      "name": "AA0073OB0101",
      "type": "ELECTRICAL_EXIT_LIGHT",
      "elevation": 700.164,
      "verticalAccuracy": 0.037,
      "horizontalAccuracy": 0.039,
      "geo": {
        "type": "Point",
        "coordinates": [
          77.0162108247222,
          43.3469187519445,
          0
        ]
      }
    },
    {
      "identifier": "8af4a1d1-4cdf-4e66-8c47-04a178d72e9e",
      "name": "AA0074OB0101",
      "type": "ELECTRICAL_EXIT_LIGHT",
      "elevation": 701.158,
      "verticalAccuracy": 0.037,
      "horizontalAccuracy": 0.039,
      "geo": {
        "type": "Point",
        "coordinates": [
          77.0133429408333,
          43.3452057358333,
          0
        ]
      }
    },
    {
      "identifier": "33340a9f-1755-4f1e-b55d-a926af3ccf1e",
      "name": "AA0078OB0101",
      "type": "ELECTRICAL_EXIT_LIGHT",
      "elevation": 695.84,
      "verticalAccuracy": 0.037,
      "horizontalAccuracy": 0.039,
      "geo": {
        "type": "Point",
        "coordinates": [
          77.012791125,
          43.3519081833333,
          0
        ]
      }
    },
    {
      "identifier": "70980098-7e5f-48a6-906b-a4f5c2e6090c",
      "name": "AA0079OB0101",
      "type": "ELECTRICAL_EXIT_LIGHT",
      "elevation": 697.151,
      "verticalAccuracy": 0.037,
      "horizontalAccuracy": 0.039,
      "geo": {
        "type": "Point",
        "coordinates": [
          77.0132224588889,
          43.3511713377778,
          0
        ]
      }
    },
    {
      "identifier": "10e1f3c7-bc02-4823-986d-38f5e2c24765",
      "name": "AA0080OB0101",
      "type": "ELECTRICAL_EXIT_LIGHT",
      "elevation": 697.544,
      "verticalAccuracy": 0.037,
      "horizontalAccuracy": 0.039,
      "geo": {
        "type": "Point",
        "coordinates": [
          77.0138004677778,
          43.3501719916667,
          0
        ]
      }
    },
    {
      "identifier": "738a75a2-c6d8-4c2d-8c94-23a2a251574f",
      "name": "AA0081OB0101",
      "type": "ELECTRICAL_EXIT_LIGHT",
      "elevation": 698.56,
      "verticalAccuracy": 0.037,
      "horizontalAccuracy": 0.039,
      "geo": {
        "type": "Point",
        "coordinates": [
          77.0142401191667,
          43.3494347886111,
          0
        ]
      }
    },
    {
      "identifier": "599ae56c-c70a-43fc-bd0c-2c98ea20b58d",
      "name": "AA0082OB0101",
      "type": "ELECTRICAL_EXIT_LIGHT",
      "elevation": 698.556,
      "verticalAccuracy": 0.037,
      "horizontalAccuracy": 0.039,
      "geo": {
        "type": "Point",
        "coordinates": [
          77.01645389,
          43.3495974047222,
          0
        ]
      }
    },
    {
      "identifier": "ee6cdf4f-778e-486c-906d-94c4d9589cf1",
      "name": "AA0083OB0101",
      "type": "ELECTRICAL_EXIT_LIGHT",
      "elevation": 698.362,
      "verticalAccuracy": 0.037,
      "horizontalAccuracy": 0.039,
      "geo": {
        "type": "Point",
        "coordinates": [
          77.017763915,
          43.3495495661111,
          0
        ]
      }
    },
    {
      "identifier": "51e91806-b665-4041-9c8f-193ef2a51aae",
      "name": "AA0084OB0101",
      "type": "ELECTRICAL_EXIT_LIGHT",
      "elevation": 698.596,
      "verticalAccuracy": 0.037,
      "horizontalAccuracy": 0.039,
      "geo": {
        "type": "Point",
        "coordinates": [
          77.0190054794444,
          43.3494859391667,
          0
        ]
      }
    },
    {
      "identifier": "c7b13b3a-71b0-4163-8ed8-c67ef42c5059",
      "name": "AA0085OB0101",
      "type": "ELECTRICAL_EXIT_LIGHT",
      "elevation": 698.551,
      "verticalAccuracy": 0.037,
      "horizontalAccuracy": 0.039,
      "geo": {
        "type": "Point",
        "coordinates": [
          77.0207541897222,
          43.3494135252778,
          0
        ]
      }
    },
    {
      "identifier": "45153d96-c670-4864-992b-4df0214c9571",
      "name": "AA0087OB0101",
      "type": "ELECTRICAL_EXIT_LIGHT",
      "elevation": 697.611,
      "verticalAccuracy": 0.037,
      "horizontalAccuracy": 0.039,
      "geo": {
        "type": "Point",
        "coordinates": [
          77.0119702911111,
          43.3490917175,
          0
        ]
      }
    },
    {
      "identifier": "b63b98ce-4af7-47e5-a7f7-43f04c599e5a",
      "name": "AA0089OB0101",
      "type": "SPIRE",
      "elevation": 699.301,
      "verticalAccuracy": 0.037,
      "horizontalAccuracy": 0.039,
      "geo": {
        "type": "Point",
        "coordinates": [
          77.011443315,
          43.3478007188889,
          0
        ]
      }
    },
    {
      "identifier": "3e925008-f924-4304-a450-ee164f02ba63",
      "name": "AA0090OB0101",
      "type": "TREE",
      "elevation": 684.832,
      "verticalAccuracy": 0.037,
      "horizontalAccuracy": 0.039,
      "geo": {
        "type": "Point",
        "coordinates": [
          77.0120788416667,
          43.3477982880556,
          0
        ]
      }
    },
    {
      "identifier": "abb62e4c-7534-475f-b443-f08257326ec9",
      "name": "AA0091OB0101",
      "type": "TREE",
      "elevation": 685.618,
      "verticalAccuracy": 0.037,
      "horizontalAccuracy": 0.039,
      "geo": {
        "type": "Point",
        "coordinates": [
          77.011613615,
          43.3482405947222,
          0
        ]
      }
    },
    {
      "identifier": "decc515e-2deb-408a-8deb-f9cdfa11bc15",
      "name": "AA0092OB0101",
      "type": "TREE",
      "elevation": 692.416,
      "verticalAccuracy": 0.037,
      "horizontalAccuracy": 0.039,
      "geo": {
        "type": "Point",
        "coordinates": [
          77.0090774852778,
          43.3502328822222,
          0
        ]
      }
    },
    {
      "identifier": "cfc4edda-7027-483e-a8e1-bb1dee04a303",
      "name": "AA0093OB0101",
      "type": "ANTENNA",
      "elevation": 695.317,
      "verticalAccuracy": 0.037,
      "horizontalAccuracy": 0.039,
      "geo": {
        "type": "Point",
        "coordinates": [
          77.0110827955556,
          43.3487850086111,
          0
        ]
      }
    },
    {
      "identifier": "b34f71f9-571b-4075-a244-fb329f3459a9",
      "name": "AA0094OB0101",
      "type": "TREE",
      "elevation": 687.796,
      "verticalAccuracy": 0.037,
      "horizontalAccuracy": 0.039,
      "geo": {
        "type": "Point",
        "coordinates": [
          77.0115533625,
          43.3498709938889,
          0
        ]
      }
    },
    {
      "identifier": "f01b71e2-6ae7-439f-831b-fc652643cade",
      "name": "AA0095OB0101",
      "type": "TREE",
      "elevation": 683.738,
      "verticalAccuracy": 0.037,
      "horizontalAccuracy": 0.039,
      "geo": {
        "type": "Point",
        "coordinates": [
          77.0131219155556,
          43.3511136536111,
          0
        ]
      }
    },
    {
      "identifier": "d61380b5-644b-42f8-8563-eb0e84ace447",
      "name": "AA0096OB0101",
      "type": null,
      "elevation": 687.547,
      "verticalAccuracy": 0.037,
      "horizontalAccuracy": 0.039,
      "geo": {
        "type": "Point",
        "coordinates": [
          77.0135759225,
          43.35114398,
          0
        ]
      }
    },
    {
      "identifier": "6faad465-9965-4ea4-b285-ac10ef303282",
      "name": "AA0105OB0101",
      "type": "SIGN",
      "elevation": 676.648,
      "verticalAccuracy": 0.037,
      "horizontalAccuracy": 0.039,
      "geo": {
        "type": "Point",
        "coordinates": [
          77.0155958266667,
          43.3477345186111,
          0
        ]
      }
    },
    {
      "identifier": "f418b72d-1e5c-47f5-90d9-07d19118217f",
      "name": "AA0106OB0101",
      "type": "SIGN",
      "elevation": 675.986,
      "verticalAccuracy": 0.037,
      "horizontalAccuracy": 0.039,
      "geo": {
        "type": "Point",
        "coordinates": [
          77.0146680488889,
          43.3493646063889,
          0
        ]
      }
    },
    {
      "identifier": "f85e8a76-bf82-4889-8b27-bfd4349cfa0b",
      "name": "AA0107OB0101",
      "type": "SIGN",
      "elevation": 675.993,
      "verticalAccuracy": 0.037,
      "horizontalAccuracy": 0.039,
      "geo": {
        "type": "Point",
        "coordinates": [
          77.0146812775,
          43.3493687672222,
          0
        ]
      }
    },
    {
      "identifier": "434ea769-7adc-4a0d-9dd1-c773f6194ea6",
      "name": "AA0108OB0101",
      "type": null,
      "elevation": 675.202,
      "verticalAccuracy": 0.037,
      "horizontalAccuracy": 0.039,
      "geo": {
        "type": "Point",
        "coordinates": [
          77.0145709283333,
          43.3497373541667,
          0
        ]
      }
    },
    {
      "identifier": "b3cddf48-87cf-4699-930d-2ba508af7908",
      "name": "AA0109OB0101",
      "type": "SIGN",
      "elevation": 675.302,
      "verticalAccuracy": 0.037,
      "horizontalAccuracy": 0.039,
      "geo": {
        "type": "Point",
        "coordinates": [
          77.0151197844445,
          43.3498638880556,
          0
        ]
      }
    },
    {
      "identifier": "388bb150-4b61-435b-9089-3a5ef7e1f0c7",
      "name": "AA0110OB0101",
      "type": "SIGN",
      "elevation": 675.437,
      "verticalAccuracy": 0.037,
      "horizontalAccuracy": 0.039,
      "geo": {
        "type": "Point",
        "coordinates": [
          77.0152098925,
          43.3499585816667,
          0
        ]
      }
    },
    {
      "identifier": "06a233ed-cfa3-4ce9-89dd-b10cea9395ad",
      "name": "AA0118OB0101",
      "type": "BUILDING",
      "elevation": 678.548,
      "verticalAccuracy": 0.037,
      "horizontalAccuracy": 0.039,
      "geo": {
        "type": "Point",
        "coordinates": [
          77.0132751872222,
          43.3452991591667,
          0
        ]
      }
    },
    {
      "identifier": "bdbad0e8-b1eb-4ea5-a34e-5e06a36422e7",
      "name": "AA0123OB0101",
      "type": "ELECTRICAL_EXIT_LIGHT",
      "elevation": 696.931,
      "verticalAccuracy": 0.037,
      "horizontalAccuracy": 0.039,
      "geo": {
        "type": "Point",
        "coordinates": [
          77.0130744108333,
          43.3431525416667,
          0
        ]
      }
    },
    {
      "identifier": "9b10cb16-f32d-46f7-9edf-6ce9c035f9eb",
      "name": "AA0124OB0101",
      "type": "ELECTRICAL_EXIT_LIGHT",
      "elevation": 687.946,
      "verticalAccuracy": 0.037,
      "horizontalAccuracy": 0.039,
      "geo": {
        "type": "Point",
        "coordinates": [
          77.0150619569445,
          43.3433300547222,
          0
        ]
      }
    },
    {
      "identifier": "23c5edb9-95ff-4e99-99d9-f2e9cc06800b",
      "name": "AA0125OB0101",
      "type": "ELECTRICAL_EXIT_LIGHT",
      "elevation": 687.833,
      "verticalAccuracy": 0.037,
      "horizontalAccuracy": 0.039,
      "geo": {
        "type": "Point",
        "coordinates": [
          77.0151201905556,
          43.3433316125,
          0
        ]
      }
    },
    {
      "identifier": "5b658419-cb0d-4171-a184-93aace3ce874",
      "name": "AA0126OB0101",
      "type": "ELECTRICAL_EXIT_LIGHT",
      "elevation": 699.853,
      "verticalAccuracy": 0.037,
      "horizontalAccuracy": 0.039,
      "geo": {
        "type": "Point",
        "coordinates": [
          77.0157109902778,
          43.3477531708333,
          0
        ]
      }
    },
    {
      "identifier": "88fafb2c-ab46-4183-ab9d-afac30b8f6f0",
      "name": "AA0131OB0101",
      "type": null,
      "elevation": 679.097,
      "verticalAccuracy": 0.037,
      "horizontalAccuracy": 0.039,
      "geo": {
        "type": "Point",
        "coordinates": [
          77.0124710375,
          43.3529032002778,
          0
        ]
      }
    },
    {
      "identifier": "ca921628-5156-4f6a-b5d3-05c0cc3eb6da",
      "name": "AA0132OB0101",
      "type": "STACK",
      "elevation": 679.139,
      "verticalAccuracy": 0.037,
      "horizontalAccuracy": 0.039,
      "geo": {
        "type": "Point",
        "coordinates": [
          77.0124617586111,
          43.3529177613889,
          0
        ]
      }
    },
    {
      "identifier": "76086206-a9d0-4e56-a073-fa0989987edd",
      "name": "AA0136OB0101",
      "type": "TREE",
      "elevation": 676.523,
      "verticalAccuracy": 0.037,
      "horizontalAccuracy": 0.039,
      "geo": {
        "type": "Point",
        "coordinates": [
          77.0125875563889,
          43.3524232238889,
          0
        ]
      }
    },
    {
      "identifier": "72d6e642-3dc3-4676-b5c0-298d69db91d7",
      "name": "AA0137OB0101",
      "type": "TREE",
      "elevation": 677.461,
      "verticalAccuracy": 0.037,
      "horizontalAccuracy": 0.039,
      "geo": {
        "type": "Point",
        "coordinates": [
          77.0125936144444,
          43.3522839933333,
          0
        ]
      }
    },
    {
      "identifier": "9e337e39-2ca5-4692-a65a-f162b927bf26",
      "name": "AA0138OB0101",
      "type": "BUILDING",
      "elevation": 673.846,
      "verticalAccuracy": 0.037,
      "horizontalAccuracy": 0.039,
      "geo": {
        "type": "Point",
        "coordinates": [
          77.0130423019444,
          43.352109525,
          0
        ]
      }
    },
    {
      "identifier": "ab3056da-ca4f-4b42-b865-9e1969498fc9",
      "name": "AA0139OB0101",
      "type": "ELECTRICAL_EXIT_LIGHT",
      "elevation": 673.467,
      "verticalAccuracy": 0.037,
      "horizontalAccuracy": 0.039,
      "geo": {
        "type": "Point",
        "coordinates": [
          77.0128646722222,
          43.3518598955556,
          0
        ]
      }
    },
    {
      "identifier": "b3bf95a9-03f6-4b48-a9be-380fd423964c",
      "name": "AA0140OB0101",
      "type": "TREE",
      "elevation": 673.66,
      "verticalAccuracy": 0.037,
      "horizontalAccuracy": 0.039,
      "geo": {
        "type": "Point",
        "coordinates": [
          77.0127155452778,
          43.3517710830556,
          0
        ]
      }
    },
    {
      "identifier": "6821a6d0-4339-4ad4-b609-67e791f6584d",
      "name": "AA0147OB0101",
      "type": "TREE",
      "elevation": 672.466,
      "verticalAccuracy": 0.037,
      "horizontalAccuracy": 0.039,
      "geo": {
        "type": "Point",
        "coordinates": [
          77.0123108875,
          43.3527019569444,
          0
        ]
      }
    },
    {
      "identifier": "b65e72c5-6a70-4e3c-95a0-53020287f87c",
      "name": "AA0148OB0101",
      "type": "TREE",
      "elevation": 675.792,
      "verticalAccuracy": 0.037,
      "horizontalAccuracy": 0.039,
      "geo": {
        "type": "Point",
        "coordinates": [
          77.0122697975,
          43.3527765958333,
          0
        ]
      }
    },
    {
      "identifier": "9846442a-d746-4590-91cf-345863edc25c",
      "name": "AA0150OB0201",
      "type": "ELECTRICAL_EXIT_LIGHT",
      "elevation": 692.766,
      "verticalAccuracy": 0.037,
      "horizontalAccuracy": 0.039,
      "geo": {
        "type": "Point",
        "coordinates": [
          77.0108423033333,
          43.3512019347222,
          0
        ]
      }
    },
    {
      "identifier": "850e4dd4-01be-47aa-9409-ffd385d11b6b",
      "name": "AA0153OB0101",
      "type": "ELECTRICAL_EXIT_LIGHT",
      "elevation": 689.398,
      "verticalAccuracy": 0.037,
      "horizontalAccuracy": 0.039,
      "geo": {
        "type": "Point",
        "coordinates": [
          77.0131656041667,
          43.3429569041667,
          0
        ]
      }
    },
    {
      "identifier": "1ae8ad46-bfed-4538-a123-4fbd1c281506",
      "name": "AA0154OB0101",
      "type": "ELECTRICAL_EXIT_LIGHT",
      "elevation": 688.711,
      "verticalAccuracy": 0.037,
      "horizontalAccuracy": 0.039,
      "geo": {
        "type": "Point",
        "coordinates": [
          77.0126203152778,
          43.3438684391667,
          0
        ]
      }
    },
    {
      "identifier": "e3a16acd-646b-4452-93af-215baab23302",
      "name": "AA0155OB0101",
      "type": "ELECTRICAL_EXIT_LIGHT",
      "elevation": 688.307,
      "verticalAccuracy": 0.037,
      "horizontalAccuracy": 0.039,
      "geo": {
        "type": "Point",
        "coordinates": [
          77.0124531847222,
          43.3441574722222,
          0
        ]
      }
    },
    {
      "identifier": "51f0f39c-e752-4d3f-b6ee-47f853ee86a7",
      "name": "AA0156OB0109",
      "type": "BUILDING",
      "elevation": 699.863,
      "verticalAccuracy": 0.037,
      "horizontalAccuracy": 0.039,
      "geo": {
        "type": "Point",
        "coordinates": [
          77.0126736897222,
          43.3436449422222,
          0
        ]
      }
    },
    {
      "identifier": "3d984050-08cd-46e9-83c8-7db501b0c41a",
      "name": "AA0161OB0101",
      "type": "TREE",
      "elevation": 683.444,
      "verticalAccuracy": 0.037,
      "horizontalAccuracy": 0.039,
      "geo": {
        "type": "Point",
        "coordinates": [
          77.0127593172222,
          43.3447530513889,
          0
        ]
      }
    },
    {
      "identifier": "e8f023d4-a5d5-409f-8c2c-2dbe833374a1",
      "name": "AA0162OB0101",
      "type": "TREE",
      "elevation": 683.226,
      "verticalAccuracy": 0.037,
      "horizontalAccuracy": 0.039,
      "geo": {
        "type": "Point",
        "coordinates": [
          77.0127880444444,
          43.3447058138889,
          0
        ]
      }
    },
    {
      "identifier": "4cf539ea-c67f-4e9c-b09f-108a57aee899",
      "name": "AA0164OB0101",
      "type": "TREE",
      "elevation": 682.823,
      "verticalAccuracy": 0.037,
      "horizontalAccuracy": 0.039,
      "geo": {
        "type": "Point",
        "coordinates": [
          77.0123004977778,
          43.3445509786111,
          0
        ]
      }
    },
    {
      "identifier": "519aef3f-864e-47df-ad9a-4a5a3953a0c0",
      "name": "AA0165OB0101",
      "type": "ELECTRICAL_EXIT_LIGHT",
      "elevation": 685.25,
      "verticalAccuracy": 0.037,
      "horizontalAccuracy": 0.039,
      "geo": {
        "type": "Point",
        "coordinates": [
          77.0122860372222,
          43.3448727827778,
          0
        ]
      }
    },
    {
      "identifier": "8dfd2f85-d31c-4f66-ae02-9726792c4fe5",
      "name": "AA0166OB0101",
      "type": "ELECTRICAL_EXIT_LIGHT",
      "elevation": 684.884,
      "verticalAccuracy": 0.037,
      "horizontalAccuracy": 0.039,
      "geo": {
        "type": "Point",
        "coordinates": [
          77.0120678886111,
          43.3448052727778,
          0
        ]
      }
    },
    {
      "identifier": "235b7636-02f2-4fc3-a392-e7066acef6a8",
      "name": "AA0167OB0101",
      "type": "ELECTRICAL_EXIT_LIGHT",
      "elevation": 685.482,
      "verticalAccuracy": 0.037,
      "horizontalAccuracy": 0.039,
      "geo": {
        "type": "Point",
        "coordinates": [
          77.0118319402778,
          43.3447363466667,
          0
        ]
      }
    },
    {
      "identifier": "a5d7e7ce-6f8e-4fde-b55a-d728cf20ab59",
      "name": "AA0168OB0103",
      "type": "FENCE",
      "elevation": 678.241,
      "verticalAccuracy": 0.037,
      "horizontalAccuracy": 0.039,
      "geo": {
        "type": "Point",
        "coordinates": [
          77.0122800908333,
          43.3445270625,
          0
        ]
      }
    },
    {
      "identifier": "25e6bb76-5a9e-4e02-90e3-8491ca0c9843",
      "name": "AA0183OB0101",
      "type": "TREE",
      "elevation": 683.102,
      "verticalAccuracy": 0.037,
      "horizontalAccuracy": 0.039,
      "geo": {
        "type": "Point",
        "coordinates": [
          77.0093211027778,
          43.3542202738889,
          0
        ]
      }
    },
    {
      "identifier": "a525a8ef-8e26-460d-82b7-319db191115e",
      "name": "AA0184OB0101",
      "type": "TREE",
      "elevation": 677.913,
      "verticalAccuracy": 0.037,
      "horizontalAccuracy": 0.039,
      "geo": {
        "type": "Point",
        "coordinates": [
          77.0094884033333,
          43.3542581405556,
          0
        ]
      }
    },
    {
      "identifier": "b108bb52-145d-4e55-ba85-ecb83cef38c6",
      "name": "AA0185OB0101",
      "type": "TREE",
      "elevation": 683.277,
      "verticalAccuracy": 0.037,
      "horizontalAccuracy": 0.039,
      "geo": {
        "type": "Point",
        "coordinates": [
          77.0097162441667,
          43.3543185247222,
          0
        ]
      }
    },
    {
      "identifier": "97a7bc5f-4c42-42fb-b698-65fd7ac6eb6e",
      "name": "AA0192OB0106",
      "type": "BUILDING",
      "elevation": 686.235,
      "verticalAccuracy": 0.037,
      "horizontalAccuracy": 0.039,
      "geo": {
        "type": "Point",
        "coordinates": [
          77.0132804002778,
          43.3428259061111,
          0
        ]
      }
    },
    {
      "identifier": "ae0ea207-f9d5-4b95-844c-08a79fa86bd9",
      "name": "AA0195OB0101",
      "type": null,
      "elevation": 683.733,
      "verticalAccuracy": 0.037,
      "horizontalAccuracy": 0.039,
      "geo": {
        "type": "Point",
        "coordinates": [
          77.01352551,
          43.3422220708333,
          0
        ]
      }
    },
    {
      "identifier": "364cf161-48c1-489f-986c-6392328fc415",
      "name": "AA0199OB0101",
      "type": "ELECTRICAL_EXIT_LIGHT",
      "elevation": 695.998,
      "verticalAccuracy": 0.037,
      "horizontalAccuracy": 0.039,
      "geo": {
        "type": "Point",
        "coordinates": [
          77.0176476388889,
          43.3518859497222,
          0
        ]
      }
    },
    {
      "identifier": "35e8af96-7bfd-4b39-bbc9-3114221cf571",
      "name": "AA0200OB0101",
      "type": "ELECTRICAL_EXIT_LIGHT",
      "elevation": 696.774,
      "verticalAccuracy": 0.037,
      "horizontalAccuracy": 0.039,
      "geo": {
        "type": "Point",
        "coordinates": [
          77.0187532411111,
          43.3518149283333,
          0
        ]
      }
    },
    {
      "identifier": "2c00c564-286d-40ee-babb-7cc29a8102a4",
      "name": "AA0201OB0101",
      "type": "ELECTRICAL_EXIT_LIGHT",
      "elevation": 696.957,
      "verticalAccuracy": 0.037,
      "horizontalAccuracy": 0.039,
      "geo": {
        "type": "Point",
        "coordinates": [
          77.0201257091667,
          43.3517659597222,
          0
        ]
      }
    },
    {
      "identifier": "e4578f97-8a07-47af-aab1-3de05f2b9d53",
      "name": "AA0202OB0101",
      "type": "ELECTRICAL_EXIT_LIGHT",
      "elevation": 697.189,
      "verticalAccuracy": 0.037,
      "horizontalAccuracy": 0.039,
      "geo": {
        "type": "Point",
        "coordinates": [
          77.0215003488889,
          43.3517152075,
          0
        ]
      }
    },
    {
      "identifier": "48061b84-586e-48fe-94b9-6f2fe8e1520e",
      "name": "AA0205OB0101",
      "type": "SIGN",
      "elevation": 676.204,
      "verticalAccuracy": 0.037,
      "horizontalAccuracy": 0.039,
      "geo": {
        "type": "Point",
        "coordinates": [
          77.0217604961111,
          43.3495874516667,
          0
        ]
      }
    },
    {
      "identifier": "616588a3-05e0-4d7b-bf45-c4381d871d50",
      "name": "AA0206OB0101",
      "type": null,
      "elevation": 680.613,
      "verticalAccuracy": 0.037,
      "horizontalAccuracy": 0.039,
      "geo": {
        "type": "Point",
        "coordinates": [
          77.0469598216667,
          43.3593237325,
          0
        ]
      }
    },
    {
      "identifier": "acfb10c7-89bc-47eb-92c6-2b6f280acce3",
      "name": "AA0207OB0101",
      "type": null,
      "elevation": 681.277,
      "verticalAccuracy": 0.037,
      "horizontalAccuracy": 0.039,
      "geo": {
        "type": "Point",
        "coordinates": [
          77.04689334,
          43.3594017733333,
          0
        ]
      }
    },
    {
      "identifier": "f3263ff2-a3bd-425d-8e03-d5e577931c0b",
      "name": "AA0208OB0101",
      "type": null,
      "elevation": 680.661,
      "verticalAccuracy": 0.037,
      "horizontalAccuracy": 0.039,
      "geo": {
        "type": "Point",
        "coordinates": [
          77.0472746,
          43.3594682855556,
          0
        ]
      }
    },
    {
      "identifier": "d8d8a26f-fe13-49a1-b8f6-55121802caa9",
      "name": "AA0209OB0101",
      "type": null,
      "elevation": 681.312,
      "verticalAccuracy": 0.037,
      "horizontalAccuracy": 0.039,
      "geo": {
        "type": "Point",
        "coordinates": [
          77.0473169244444,
          43.3596101630556,
          0
        ]
      }
    },
    {
      "identifier": "1e13ca0b-3d40-4700-8c37-8cc88a16a060",
      "name": "AA0210OB0101",
      "type": null,
      "elevation": 681.605,
      "verticalAccuracy": 0.037,
      "horizontalAccuracy": 0.039,
      "geo": {
        "type": "Point",
        "coordinates": [
          77.0417443091667,
          43.3519006580556,
          0
        ]
      }
    },
    {
      "identifier": "652c7b57-c340-4b0c-99f2-1434c583dbcc",
      "name": "AA0211OB0101",
      "type": null,
      "elevation": 680.935,
      "verticalAccuracy": 0.037,
      "horizontalAccuracy": 0.039,
      "geo": {
        "type": "Point",
        "coordinates": [
          77.0409620116667,
          43.3517188122222,
          0
        ]
      }
    },
    {
      "identifier": "f01dfe4e-75df-4fc9-a280-491c2ca69522",
      "name": "AA0212OB0101",
      "type": null,
      "elevation": 681.604,
      "verticalAccuracy": 0.037,
      "horizontalAccuracy": 0.039,
      "geo": {
        "type": "Point",
        "coordinates": [
          77.0409724047222,
          43.3515308441667,
          0
        ]
      }
    },
    {
      "identifier": "5d946a81-17bc-4e44-be68-5c191abb7be1",
      "name": "AA0213OB0101",
      "type": null,
      "elevation": 680.986,
      "verticalAccuracy": 0.037,
      "horizontalAccuracy": 0.039,
      "geo": {
        "type": "Point",
        "coordinates": [
          77.0406462938889,
          43.3515721780556,
          0
        ]
      }
    },
    {
      "identifier": "f904701d-a810-4b7a-b518-ce1806345a6e",
      "name": "AA0215OB0101",
      "type": "POLE",
      "elevation": 683.741,
      "verticalAccuracy": 0.037,
      "horizontalAccuracy": 0.039,
      "geo": {
        "type": "Point",
        "coordinates": [
          77.0585029572222,
          43.3601932611111,
          0
        ]
      }
    },
    {
      "identifier": "ff49f2d8-206c-477c-a0d0-00220e230834",
      "name": "AA0216OB0101",
      "type": "POLE",
      "elevation": 683.783,
      "verticalAccuracy": 0.037,
      "horizontalAccuracy": 0.039,
      "geo": {
        "type": "Point",
        "coordinates": [
          77.0592627058333,
          43.3605737636111,
          0
        ]
      }
    },
    {
      "identifier": "dfc9e1a9-5d6c-419a-a36f-3cea17daf73b",
      "name": "AA0217OB0101",
      "type": "POLE",
      "elevation": 683.231,
      "verticalAccuracy": 0.037,
      "horizontalAccuracy": 0.039,
      "geo": {
        "type": "Point",
        "coordinates": [
          77.0591777538889,
          43.360675195,
          0
        ]
      }
    },
    {
      "identifier": "dbaee95b-d3f7-4e6d-b340-d6be4907ed9a",
      "name": "AA0218OB0101",
      "type": "POLE",
      "elevation": 683.144,
      "verticalAccuracy": 0.037,
      "horizontalAccuracy": 0.039,
      "geo": {
        "type": "Point",
        "coordinates": [
          77.05885698,
          43.3605196805556,
          0
        ]
      }
    },
    {
      "identifier": "52bf6741-f219-4d54-86f1-592020ee8d99",
      "name": "AA0219OB0101",
      "type": "POLE",
      "elevation": 682.247,
      "verticalAccuracy": 0.037,
      "horizontalAccuracy": 0.039,
      "geo": {
        "type": "Point",
        "coordinates": [
          77.0585099775,
          43.3604978813889,
          0
        ]
      }
    },
    {
      "identifier": "9b4cdba7-9fb8-4b8c-9c0e-981d4e1c6077",
      "name": "AA0220OB0101",
      "type": null,
      "elevation": 695.071,
      "verticalAccuracy": 0.037,
      "horizontalAccuracy": 0.039,
      "geo": {
        "type": "Point",
        "coordinates": [
          77.0282538875,
          43.3546443427778,
          0
        ]
      }
    },
    {
      "identifier": "d060c015-a081-456b-ad9c-1a7a47323e09",
      "name": "AA0221OB0102",
      "type": null,
      "elevation": 696.01,
      "verticalAccuracy": 0.037,
      "horizontalAccuracy": 0.039,
      "geo": {
        "type": "Point",
        "coordinates": [
          77.0285562516667,
          43.3538696325,
          0
        ]
      }
    },
    {
      "identifier": "efe7079e-0ffe-4997-aad8-f7cd50b9186d",
      "name": "AA0222OB0101",
      "type": null,
      "elevation": 690.157,
      "verticalAccuracy": 0.037,
      "horizontalAccuracy": 0.039,
      "geo": {
        "type": "Point",
        "coordinates": [
          77.0291255933333,
          43.3543226147222,
          0
        ]
      }
    },
    {
      "identifier": "cb791310-db0f-4654-84a3-eee9828024b6",
      "name": "AA0223OB0101",
      "type": null,
      "elevation": 700.06,
      "verticalAccuracy": 0.037,
      "horizontalAccuracy": 0.039,
      "geo": {
        "type": "Point",
        "coordinates": [
          77.0292991813889,
          43.3537191786111,
          0
        ]
      }
    },
    {
      "identifier": "f0ad13cb-182d-48c5-ba5f-fefe24ced88a",
      "name": "AAAERANT_0001",
      "type": "ANTENNA",
      "elevation": 680.155,
      "verticalAccuracy": 0,
      "horizontalAccuracy": 0,
      "geo": {
        "type": "Point",
        "coordinates": [
          77.0625666389,
          43.3653958611,
          0
        ]
      }
    },
    {
      "identifier": "cc705b71-bad6-4b96-9f5d-df600238e4cb",
      "name": "AAAERANT_0002",
      "type": "ANTENNA",
      "elevation": 680.049,
      "verticalAccuracy": 0,
      "horizontalAccuracy": 0,
      "geo": {
        "type": "Point",
        "coordinates": [
          77.0604937222,
          43.3643869167,
          0
        ]
      }
    },
    {
      "identifier": "7d6e5ae4-525c-4960-8c2a-1bda2f0ce7dc",
      "name": "AAAERANT_0003",
      "type": "ANTENNA",
      "elevation": 681.784,
      "verticalAccuracy": 0,
      "horizontalAccuracy": 0,
      "geo": {
        "type": "Point",
        "coordinates": [
          77.0605350556,
          43.3630211389,
          0
        ]
      }
    },
    {
      "identifier": "8f9b99cf-ce1b-4254-99e2-38521b989f19",
      "name": "AAAERANT_0004",
      "type": "ANTENNA",
      "elevation": 676.958,
      "verticalAccuracy": 0,
      "horizontalAccuracy": 0,
      "geo": {
        "type": "Point",
        "coordinates": [
          77.0315436667,
          43.3486146944,
          0
        ]
      }
    },
    {
      "identifier": "9cb43cc0-1294-476e-a53e-ca624a7cb162",
      "name": "AAAERANT_0005",
      "type": "ANTENNA",
      "elevation": 683.919,
      "verticalAccuracy": 0,
      "horizontalAccuracy": 0,
      "geo": {
        "type": "Point",
        "coordinates": [
          77.0587039444,
          43.3630121944,
          0
        ]
      }
    },
    {
      "identifier": "c0a15b45-2073-444b-86ec-40a813ff6569",
      "name": "AAAERANT_0006",
      "type": "ANTENNA",
      "elevation": 683.911,
      "verticalAccuracy": 0,
      "horizontalAccuracy": 0,
      "geo": {
        "type": "Point",
        "coordinates": [
          77.0594608056,
          43.3633923333,
          0
        ]
      }
    },
    {
      "identifier": "3d086f84-2682-469c-85a1-a1f1fa97672d",
      "name": "AAAERANT_0007",
      "type": "BUILDING",
      "elevation": 688.4,
      "verticalAccuracy": 0,
      "horizontalAccuracy": 0,
      "geo": {
        "type": "Point",
        "coordinates": [
          77.0596159722,
          43.3605958333,
          0
        ]
      }
    },
    {
      "identifier": "10c90620-bccf-4cbf-b233-18d5e089a45b",
      "name": "AAAERANT_0008",
      "type": "ANTENNA",
      "elevation": 691.058,
      "verticalAccuracy": 0,
      "horizontalAccuracy": 0,
      "geo": {
        "type": "Point",
        "coordinates": [
          77.0591783333,
          43.3603562778,
          0
        ]
      }
    },
    {
      "identifier": "b6fe13f9-fea2-4971-8caa-50708cca740f",
      "name": "AAAERANT_0009",
      "type": "BUILDING",
      "elevation": 684.824,
      "verticalAccuracy": 0,
      "horizontalAccuracy": 0,
      "geo": {
        "type": "Point",
        "coordinates": [
          77.0588453056,
          43.3598936111,
          0
        ]
      }
    },
    {
      "identifier": "30cf78e5-8d1c-4725-ad1b-0cf31f6e951a",
      "name": "AAAERANT_0010",
      "type": "POLE",
      "elevation": 685.964,
      "verticalAccuracy": 0,
      "horizontalAccuracy": 0,
      "geo": {
        "type": "Point",
        "coordinates": [
          77.0588453056,
          43.3598936111,
          0
        ]
      }
    },
    {
      "identifier": "08a57e6a-e9da-4a03-a445-dd9cdfb16a79",
      "name": "AAAERANT_0011",
      "type": "TOWER",
      "elevation": 687.364,
      "verticalAccuracy": 0,
      "horizontalAccuracy": 0,
      "geo": {
        "type": "Point",
        "coordinates": [
          77.0492703889,
          43.3555735833,
          0
        ]
      }
    },
    {
      "identifier": "e652a9be-fd5d-4a54-be4c-2b11a50e8c36",
      "name": "AAAERANT_0012",
      "type": "ANTENNA",
      "elevation": 684.185,
      "verticalAccuracy": 0,
      "horizontalAccuracy": 0,
      "geo": {
        "type": "Point",
        "coordinates": [
          77.0417952778,
          43.3518916667,
          0
        ]
      }
    },
    {
      "identifier": "f65ec287-62bb-4bf8-9875-fe0e3d4d6242",
      "name": "AAAERANT_0013",
      "type": "ANTENNA",
      "elevation": 685.549,
      "verticalAccuracy": 0,
      "horizontalAccuracy": 0,
      "geo": {
        "type": "Point",
        "coordinates": [
          77.0416461111,
          43.3512286389,
          0
        ]
      }
    },
    {
      "identifier": "7a3ea81e-e718-499b-8650-ddcf91c9e705",
      "name": "AAAERANT_0014",
      "type": "ANTENNA",
      "elevation": 685.489,
      "verticalAccuracy": 0,
      "horizontalAccuracy": 0,
      "geo": {
        "type": "Point",
        "coordinates": [
          77.0415766111,
          43.3511933333,
          0
        ]
      }
    },
    {
      "identifier": "f07a79bd-6bf1-4df3-a0cd-4a7638c1a1fa",
      "name": "AAAERANT_0015",
      "type": "DOME",
      "elevation": 682.673,
      "verticalAccuracy": 0,
      "horizontalAccuracy": 0,
      "geo": {
        "type": "Point",
        "coordinates": [
          77.0406885833,
          43.3513775833,
          0
        ]
      }
    },
    {
      "identifier": "5361dd89-eab9-4a40-b0ff-fb702f7debf0",
      "name": "AAAERANT_0016",
      "type": "POLE",
      "elevation": 683.383,
      "verticalAccuracy": 0,
      "horizontalAccuracy": 0,
      "geo": {
        "type": "Point",
        "coordinates": [
          77.0406885833,
          43.3513775833,
          0
        ]
      }
    },
    {
      "identifier": "44a0809f-cb4d-4428-8b5c-78df2f99d4ee",
      "name": "AAAERANT_0017",
      "type": "POLE",
      "elevation": 686.5,
      "verticalAccuracy": 0,
      "horizontalAccuracy": 0,
      "geo": {
        "type": "Point",
        "coordinates": [
          77.0408251389,
          43.3511135833,
          0
        ]
      }
    },
    {
      "identifier": "4cf48676-741a-4e34-8921-70ef0fdbc28d",
      "name": "AAAERANT_0018",
      "type": "FENCE",
      "elevation": 680.167,
      "verticalAccuracy": 0,
      "horizontalAccuracy": 0,
      "geo": {
        "type": "Point",
        "coordinates": [
          77.0384275833,
          43.3502148889,
          0
        ]
      }
    },
    {
      "identifier": "bb504e9b-f2cd-49f8-af73-8643387a9c4d",
      "name": "AAAERANT_0019",
      "type": "POLE",
      "elevation": 683.862,
      "verticalAccuracy": 0,
      "horizontalAccuracy": 0,
      "geo": {
        "type": "Point",
        "coordinates": [
          77.0376140556,
          43.3495886389,
          0
        ]
      }
    },
    {
      "identifier": "70a98761-55cc-4d5f-b2f0-eb702f1b75e1",
      "name": "AAAERANT_0020",
      "type": "TOWER",
      "elevation": 682.343,
      "verticalAccuracy": 0,
      "horizontalAccuracy": 0,
      "geo": {
        "type": "Point",
        "coordinates": [
          77.0331923889,
          43.3477055556,
          0
        ]
      }
    },
    {
      "identifier": "229c0d4f-81aa-4152-9b45-3448edfe9002",
      "name": "AAAERANT_0021",
      "type": "ANTENNA",
      "elevation": 681.266,
      "verticalAccuracy": 0,
      "horizontalAccuracy": 0,
      "geo": {
        "type": "Point",
        "coordinates": [
          77.0383343056,
          43.3528781389,
          0
        ]
      }
    },
    {
      "identifier": "4f02c4fe-5f17-49ef-b869-6383f30df86e",
      "name": "AAAERANT_0022",
      "type": "ANTENNA",
      "elevation": 681.437,
      "verticalAccuracy": 0,
      "horizontalAccuracy": 0,
      "geo": {
        "type": "Point",
        "coordinates": [
          77.0390966111,
          43.3532552778,
          0
        ]
      }
    },
    {
      "identifier": "4a8ada9f-77f6-4de0-acd0-94445e8481d8",
      "name": "AAAERANT_0023",
      "type": "TREE",
      "elevation": 682.101,
      "verticalAccuracy": 0,
      "horizontalAccuracy": 0,
      "geo": {
        "type": "Point",
        "coordinates": [
          77.0595311944,
          43.3607258056,
          0
        ]
      }
    },
    {
      "identifier": "77e10241-1d45-45f9-892a-e7f8b8d8af84",
      "name": "AAAERANT_0024",
      "type": "TREE",
      "elevation": 681.332,
      "verticalAccuracy": 0,
      "horizontalAccuracy": 0,
      "geo": {
        "type": "Point",
        "coordinates": [
          77.0596770278,
          43.3607888611,
          0
        ]
      }
    },
    {
      "identifier": "0d1aa148-3c33-4c24-8290-d8eefcbaea0e",
      "name": "AAAERANT_0025",
      "type": "POLE",
      "elevation": 687.934,
      "verticalAccuracy": 0,
      "horizontalAccuracy": 0,
      "geo": {
        "type": "Point",
        "coordinates": [
          77.0526256111,
          43.3572215278,
          0
        ]
      }
    },
    {
      "identifier": "59257b9f-ad6b-4eab-8862-e895a95642ef",
      "name": "AAAERANT_0026",
      "type": "POLE",
      "elevation": 686.873,
      "verticalAccuracy": 0,
      "horizontalAccuracy": 0,
      "geo": {
        "type": "Point",
        "coordinates": [
          77.0417568889,
          43.3518452222,
          0
        ]
      }
    },
    {
      "identifier": "26e5bbb5-21f5-4f2f-a59e-052d4e3da8ef",
      "name": "AAAERANT_0027",
      "type": "ANTENNA",
      "elevation": 684.466,
      "verticalAccuracy": 0,
      "horizontalAccuracy": 0,
      "geo": {
        "type": "Point",
        "coordinates": [
          77.0598787778,
          43.3606429167,
          0
        ]
      }
    },
    {
      "identifier": "7f74e9c4-b965-4320-8d8f-9db8edd3d41c",
      "name": "AADME05LNV01",
      "type": "NAVAID",
      "elevation": 677,
      "verticalAccuracy": 0,
      "horizontalAccuracy": 0,
      "geo": {
        "type": "Point",
        "coordinates": [
          77.0274166666667,
          43.34975,
          0
        ]
      }
    },
    {
      "identifier": "c6d58159-dd2a-471b-b671-29f5527f96fb",
      "name": "AADME05RNV01",
      "type": "NAVAID",
      "elevation": 681,
      "verticalAccuracy": 0,
      "horizontalAccuracy": 0,
      "geo": {
        "type": "Point",
        "coordinates": [
          77.0215555555556,
          43.3418333333333,
          0
        ]
      }
    },
    {
      "identifier": "90ef9f45-40d7-49d5-949d-ec28c44782b0",
      "name": "AADME23LNV01",
      "type": "NAVAID",
      "elevation": 684,
      "verticalAccuracy": 0,
      "horizontalAccuracy": 0,
      "geo": {
        "type": "Point",
        "coordinates": [
          77.0601111111111,
          43.36075,
          0
        ]
      }
    },
    {
      "identifier": "294f08f1-5ae9-4b94-8fcf-1b3526f843ac",
      "name": "AADME23RNV01",
      "type": "NAVAID",
      "elevation": 680,
      "verticalAccuracy": 0,
      "horizontalAccuracy": 0,
      "geo": {
        "type": "Point",
        "coordinates": [
          77.0671111111111,
          43.3696388888889,
          0
        ]
      }
    },
    {
      "identifier": "858f4aa9-f73a-47bd-ad53-d77a0239981e",
      "name": "AADVOR23LNV01",
      "type": "NAVAID",
      "elevation": 681,
      "verticalAccuracy": 0,
      "horizontalAccuracy": 0,
      "geo": {
        "type": "Point",
        "coordinates": [
          77.08525,
          43.3748055555556,
          0
        ]
      }
    },
    {
      "identifier": "f6fc4257-75c4-44f2-b25c-12641a598a70",
      "name": "AADVOR23RNV01",
      "type": "NAVAID",
      "elevation": 679,
      "verticalAccuracy": 0,
      "horizontalAccuracy": 0,
      "geo": {
        "type": "Point",
        "coordinates": [
          77.0841111111111,
          43.3765277777778,
          0
        ]
      }
    },
    {
      "identifier": "3a8061b9-9457-4fea-b3d0-2af99906e7cb",
      "name": "AADVORDME23LNV01",
      "type": "NAVAID",
      "elevation": 684,
      "verticalAccuracy": 0,
      "horizontalAccuracy": 0,
      "geo": {
        "type": "Point",
        "coordinates": [
          77.0854166666667,
          43.37475,
          0
        ]
      }
    },
    {
      "identifier": "13f5e274-4875-4617-9fe0-7787d64be714",
      "name": "AADVORDME23RNV01",
      "type": "NAVAID",
      "elevation": 682,
      "verticalAccuracy": 0,
      "horizontalAccuracy": 0,
      "geo": {
        "type": "Point",
        "coordinates": [
          77.08425,
          43.3765,
          0
        ]
      }
    },
    {
      "identifier": "8d285329-650a-4203-92fc-a76b057fd736",
      "name": "AAGP05LNV01",
      "type": "NAVAID",
      "elevation": 675,
      "verticalAccuracy": 0,
      "horizontalAccuracy": 0,
      "geo": {
        "type": "Point",
        "coordinates": [
          77.0274166666667,
          43.34975,
          0
        ]
      }
    },
    {
      "identifier": "92cd888d-3d21-4a8a-9c47-30997c00a88a",
      "name": "AAGP05RNV01",
      "type": "NAVAID",
      "elevation": 678,
      "verticalAccuracy": 0,
      "horizontalAccuracy": 0,
      "geo": {
        "type": "Point",
        "coordinates": [
          77.0215555555556,
          43.3418333333333,
          0
        ]
      }
    },
    {
      "identifier": "7cc05f07-dcd7-4425-b099-59fe2f562464",
      "name": "AAGP23LNV01",
      "type": "NAVAID",
      "elevation": 681,
      "verticalAccuracy": 0,
      "horizontalAccuracy": 0,
      "geo": {
        "type": "Point",
        "coordinates": [
          77.0601111111111,
          43.36075,
          0
        ]
      }
    },
    {
      "identifier": "cb45c834-c89b-42c2-a2db-f35c5556526a",
      "name": "AAGP23RNV01",
      "type": "NAVAID",
      "elevation": 678,
      "verticalAccuracy": 0,
      "horizontalAccuracy": 0,
      "geo": {
        "type": "Point",
        "coordinates": [
          77.0671111111111,
          43.3696388888889,
          0
        ]
      }
    },
    {
      "identifier": "476b10f6-ea10-4717-bafe-5eb4abdd921d",
      "name": "AAKAN0001",
      "type": "ANTENNA",
      "elevation": 862,
      "verticalAccuracy": 0,
      "horizontalAccuracy": 0,
      "geo": {
        "type": "Point",
        "coordinates": [
          77.1473163889,
          43.3513558333,
          0
        ]
      }
    },
    {
      "identifier": "d549d78a-c289-4011-8a44-5ebca29bae69",
      "name": "AAKAN0002",
      "type": "BUILDING",
      "elevation": 733.85,
      "verticalAccuracy": 0,
      "horizontalAccuracy": 0,
      "geo": {
        "type": "Point",
        "coordinates": [
          77.0376822778,
          43.3129573056,
          0
        ]
      }
    },
    {
      "identifier": "6c83fac3-c9cc-42ac-b5ec-2a4b8a65e5ee",
      "name": "AAKAN0007",
      "type": "BUILDING",
      "elevation": 733.2,
      "verticalAccuracy": 0,
      "horizontalAccuracy": 0,
      "geo": {
        "type": "Point",
        "coordinates": [
          77.0376822778,
          43.3129573056,
          0
        ]
      }
    },
    {
      "identifier": "3054cf9e-d5b6-4c16-8487-f074e36d17b3",
      "name": "AAKAN0008",
      "type": "ANTENNA",
      "elevation": 687.581,
      "verticalAccuracy": 0,
      "horizontalAccuracy": 0,
      "geo": {
        "type": "Point",
        "coordinates": [
          77.0120476389,
          43.3385591944,
          0
        ]
      }
    },
    {
      "identifier": "fd9d9eb4-d634-425e-b96a-9a967cb4bfd6",
      "name": "AAKAN0009",
      "type": "ANTENNA",
      "elevation": 684.834,
      "verticalAccuracy": 0,
      "horizontalAccuracy": 0,
      "geo": {
        "type": "Point",
        "coordinates": [
          77.0130601111,
          43.33906125,
          0
        ]
      }
    },
    {
      "identifier": "867521b3-9bd5-439b-b2c5-1f9b0991bbe4",
      "name": "AAKAN0010",
      "type": "AG_EQUIP",
      "elevation": 687.095,
      "verticalAccuracy": 0,
      "horizontalAccuracy": 0,
      "geo": {
        "type": "Point",
        "coordinates": [
          77.0115091944,
          43.3387778333,
          0
        ]
      }
    },
    {
      "identifier": "c87b5553-f34a-4fc6-8d6a-3778f5d56cfd",
      "name": "AAKAN0011",
      "type": null,
      "elevation": 686.887,
      "verticalAccuracy": 0,
      "horizontalAccuracy": 0,
      "geo": {
        "type": "Point",
        "coordinates": [
          77.0113663056,
          43.3389206667,
          0
        ]
      }
    },
    {
      "identifier": "6e073032-3ce7-435a-86b0-70aa31275a7d",
      "name": "AAKAN0012",
      "type": "ANTENNA",
      "elevation": 701.141,
      "verticalAccuracy": 0,
      "horizontalAccuracy": 0,
      "geo": {
        "type": "Point",
        "coordinates": [
          77.0599825556,
          43.36067975,
          0
        ]
      }
    },
    {
      "identifier": "74cdc2cc-e7f9-4239-8316-ca8595e9fe74",
      "name": "AAKAN0013",
      "type": null,
      "elevation": 690.943,
      "verticalAccuracy": 0,
      "horizontalAccuracy": 0,
      "geo": {
        "type": "Point",
        "coordinates": [
          77.0608541389,
          43.3611112778,
          0
        ]
      }
    },
    {
      "identifier": "16e9641d-a9d2-4994-a23c-3ef5f96b59f3",
      "name": "AAKAN0014",
      "type": null,
      "elevation": 688.988,
      "verticalAccuracy": 0,
      "horizontalAccuracy": 0,
      "geo": {
        "type": "Point",
        "coordinates": [
          77.0600315556,
          43.3604031667,
          0
        ]
      }
    },
    {
      "identifier": "f4c18945-c609-41f7-8a89-9000f8f21c60",
      "name": "AAKAN0015",
      "type": null,
      "elevation": 689.038,
      "verticalAccuracy": 0,
      "horizontalAccuracy": 0,
      "geo": {
        "type": "Point",
        "coordinates": [
          77.060151,
          43.3604482778,
          0
        ]
      }
    },
    {
      "identifier": "58ca7755-8f28-4a6b-b58d-cf77ec8b08e4",
      "name": "AAKAN0016",
      "type": null,
      "elevation": 690.941,
      "verticalAccuracy": 0,
      "horizontalAccuracy": 0,
      "geo": {
        "type": "Point",
        "coordinates": [
          77.07308375,
          43.3687956111,
          0
        ]
      }
    },
    {
      "identifier": "22314ff0-91da-44f8-8ce0-e67e4e78026c",
      "name": "AAKAN0017",
      "type": null,
      "elevation": 683.708,
      "verticalAccuracy": 0,
      "horizontalAccuracy": 0,
      "geo": {
        "type": "Point",
        "coordinates": [
          77.0174920833,
          43.3442777778,
          0
        ]
      }
    },
    {
      "identifier": "7ed7b853-8d56-4cfd-a84a-2810dafc7480",
      "name": "AAKAN0018",
      "type": null,
      "elevation": 681.24,
      "verticalAccuracy": 0,
      "horizontalAccuracy": 0,
      "geo": {
        "type": "Point",
        "coordinates": [
          77.0770448889,
          43.3737703333,
          0
        ]
      }
    },
    {
      "identifier": "c3c42388-69e8-47ff-95ad-22ec11c13e29",
      "name": "AAKAN0019",
      "type": null,
      "elevation": 685.174,
      "verticalAccuracy": 0,
      "horizontalAccuracy": 0,
      "geo": {
        "type": "Point",
        "coordinates": [
          77.0670664722,
          43.3696127222,
          0
        ]
      }
    },
    {
      "identifier": "575da864-e282-4755-9689-059d4ae97eeb",
      "name": "AAKAN0020",
      "type": null,
      "elevation": 682.213,
      "verticalAccuracy": 0,
      "horizontalAccuracy": 0,
      "geo": {
        "type": "Point",
        "coordinates": [
          77.0275199167,
          43.3498086111,
          0
        ]
      }
    },
    {
      "identifier": "9d06ec21-e768-45d9-9311-e3b4efb467da",
      "name": "AAKAN0021",
      "type": null,
      "elevation": 679.65,
      "verticalAccuracy": 0,
      "horizontalAccuracy": 0,
      "geo": {
        "type": "Point",
        "coordinates": [
          77.0841540833,
          43.3765181944,
          0
        ]
      }
    },
    {
      "identifier": "f4e49b59-4929-4d0f-ad85-062f9cbd13d2",
      "name": "AAKAN0030",
      "type": "POLE",
      "elevation": 673.904,
      "verticalAccuracy": 0,
      "horizontalAccuracy": 0,
      "geo": {
        "type": "Point",
        "coordinates": [
          77.0214222222,
          43.3596194444,
          0
        ]
      }
    },
    {
      "identifier": "63ac5b94-af78-4041-b4e7-5dda3de87dcd",
      "name": "AAKAN0031",
      "type": "POLE",
      "elevation": 673.668,
      "verticalAccuracy": 0,
      "horizontalAccuracy": 0,
      "geo": {
        "type": "Point",
        "coordinates": [
          77.0220472222,
          43.3594444444,
          0
        ]
      }
    },
    {
      "identifier": "e5c5c752-dce7-4ddf-92e8-005e752d5107",
      "name": "AAKAN0032",
      "type": "POLE",
      "elevation": 675.895,
      "verticalAccuracy": 0,
      "horizontalAccuracy": 0,
      "geo": {
        "type": "Point",
        "coordinates": [
          77.0226111111,
          43.3593777778,
          0
        ]
      }
    },
    {
      "identifier": "9421d120-13f1-4c18-a149-8587a99276b4",
      "name": "AAKAN0033",
      "type": "POLE",
      "elevation": 674.965,
      "verticalAccuracy": 0,
      "horizontalAccuracy": 0,
      "geo": {
        "type": "Point",
        "coordinates": [
          77.0229916667,
          43.3589305556,
          0
        ]
      }
    },
    {
      "identifier": "dd826d60-c8d4-4fd9-b79b-8ca71cffd274",
      "name": "AAKAN0034",
      "type": "POLE",
      "elevation": 675.314,
      "verticalAccuracy": 0,
      "horizontalAccuracy": 0,
      "geo": {
        "type": "Point",
        "coordinates": [
          77.024025,
          43.3583388889,
          0
        ]
      }
    },
    {
      "identifier": "b0564f86-9680-4c70-b50e-d52042badc36",
      "name": "AAKAN0035",
      "type": "POLE",
      "elevation": 676.107,
      "verticalAccuracy": 0,
      "horizontalAccuracy": 0,
      "geo": {
        "type": "Point",
        "coordinates": [
          77.02515,
          43.357675,
          0
        ]
      }
    },
    {
      "identifier": "30ad1ec5-6a17-4e95-b049-35e3f6c6518d",
      "name": "AAKAN0036",
      "type": "POLE",
      "elevation": 676.891,
      "verticalAccuracy": 0,
      "horizontalAccuracy": 0,
      "geo": {
        "type": "Point",
        "coordinates": [
          77.0261472222,
          43.3568527778,
          0
        ]
      }
    },
    {
      "identifier": "916bcadc-cee9-4cb2-9615-cd6b5592744f",
      "name": "AAKAN0037",
      "type": "POLE",
      "elevation": 678.082,
      "verticalAccuracy": 0,
      "horizontalAccuracy": 0,
      "geo": {
        "type": "Point",
        "coordinates": [
          77.027175,
          43.3560861111,
          0
        ]
      }
    },
    {
      "identifier": "41e9cf4d-297c-44cc-a17d-93eedc4342ce",
      "name": "AAKAN0038",
      "type": "POLE",
      "elevation": 678.591,
      "verticalAccuracy": 0,
      "horizontalAccuracy": 0,
      "geo": {
        "type": "Point",
        "coordinates": [
          77.0282305556,
          43.3553027778,
          0
        ]
      }
    },
    {
      "identifier": "486a0783-3eb9-4fc4-86b9-39e6cad0ac88",
      "name": "AAKAN0039",
      "type": "POLE",
      "elevation": 679.102,
      "verticalAccuracy": 0,
      "horizontalAccuracy": 0,
      "geo": {
        "type": "Point",
        "coordinates": [
          77.0287055556,
          43.355425,
          0
        ]
      }
    },
    {
      "identifier": "d7ddebe4-4d46-4721-84ae-517448d0c0a2",
      "name": "AAKAN0040",
      "type": "POLE",
      "elevation": 679.451,
      "verticalAccuracy": 0,
      "horizontalAccuracy": 0,
      "geo": {
        "type": "Point",
        "coordinates": [
          77.029325,
          43.3546861111,
          0
        ]
      }
    },
    {
      "identifier": "4666b7d0-ac55-4b8b-99ae-8d647580bda4",
      "name": "AAKAN0041",
      "type": "POLE",
      "elevation": 679.985,
      "verticalAccuracy": 0,
      "horizontalAccuracy": 0,
      "geo": {
        "type": "Point",
        "coordinates": [
          77.029375,
          43.3542638889,
          0
        ]
      }
    },
    {
      "identifier": "40c1e885-e7d5-4b70-93c1-6d1a65278fe9",
      "name": "AAKAN0042",
      "type": "POLE",
      "elevation": 680.219,
      "verticalAccuracy": 0,
      "horizontalAccuracy": 0,
      "geo": {
        "type": "Point",
        "coordinates": [
          77.0294527778,
          43.3537222222,
          0
        ]
      }
    },
    {
      "identifier": "1541964d-7c62-4ed0-a2f8-01c0a6d4ce52",
      "name": "AAKAN0043",
      "type": "POLE",
      "elevation": 680.142,
      "verticalAccuracy": 0,
      "horizontalAccuracy": 0,
      "geo": {
        "type": "Point",
        "coordinates": [
          77.0295861111,
          43.3539416667,
          0
        ]
      }
    },
    {
      "identifier": "d930a7e0-45f7-4503-90e9-b101c56ccf5b",
      "name": "AAKAN0044",
      "type": "POLE",
      "elevation": 680.555,
      "verticalAccuracy": 0,
      "horizontalAccuracy": 0,
      "geo": {
        "type": "Point",
        "coordinates": [
          77.0288444444,
          43.3535222222,
          0
        ]
      }
    },
    {
      "identifier": "2cd4213d-3e3b-4fa1-a753-e62e6bae59e7",
      "name": "AAKAN0045",
      "type": "POLE",
      "elevation": 681.744,
      "verticalAccuracy": 0,
      "horizontalAccuracy": 0,
      "geo": {
        "type": "Point",
        "coordinates": [
          77.0290638889,
          43.3524472222,
          0
        ]
      }
    },
    {
      "identifier": "f8781b4e-958a-4ecf-972f-f45a9cb89e26",
      "name": "AAKAN0046",
      "type": "POLE",
      "elevation": 679.984,
      "verticalAccuracy": 0,
      "horizontalAccuracy": 0,
      "geo": {
        "type": "Point",
        "coordinates": [
          77.0297527778,
          43.3527416667,
          0
        ]
      }
    },
    {
      "identifier": "3c7daf6f-9514-4e1e-8d8f-4edb3d50b9ff",
      "name": "AAKAN0047",
      "type": "POLE",
      "elevation": 679.005,
      "verticalAccuracy": 0,
      "horizontalAccuracy": 0,
      "geo": {
        "type": "Point",
        "coordinates": [
          77.0309777778,
          43.3533638889,
          0
        ]
      }
    },
    {
      "identifier": "359b4c44-ee08-4c74-9b4a-63c077c05f4f",
      "name": "AAKAN0048",
      "type": "POLE",
      "elevation": 680.388,
      "verticalAccuracy": 0,
      "horizontalAccuracy": 0,
      "geo": {
        "type": "Point",
        "coordinates": [
          77.0322222222,
          43.3539583333,
          0
        ]
      }
    },
    {
      "identifier": "dbd3e141-055e-4c08-917d-73cf3015b0ac",
      "name": "AAKAN0049",
      "type": "POLE",
      "elevation": 681.786,
      "verticalAccuracy": 0,
      "horizontalAccuracy": 0,
      "geo": {
        "type": "Point",
        "coordinates": [
          77.0334444444,
          43.3545583333,
          0
        ]
      }
    },
    {
      "identifier": "ffe37bbc-f44a-45fa-9803-7dee6e4911c9",
      "name": "AAKAN0050",
      "type": "POLE",
      "elevation": 682.508,
      "verticalAccuracy": 0,
      "horizontalAccuracy": 0,
      "geo": {
        "type": "Point",
        "coordinates": [
          77.0346916667,
          43.3551777778,
          0
        ]
      }
    },
    {
      "identifier": "76483c03-0042-4fdc-bb8c-51e7fb77192e",
      "name": "AAKAN0051",
      "type": "POLE",
      "elevation": 681.603,
      "verticalAccuracy": 0,
      "horizontalAccuracy": 0,
      "geo": {
        "type": "Point",
        "coordinates": [
          77.0344,
          43.355475,
          0
        ]
      }
    },
    {
      "identifier": "cdba7ebd-5255-4664-91b9-b37c1547590c",
      "name": "AAKAN0052",
      "type": "POLE",
      "elevation": 681.572,
      "verticalAccuracy": 0,
      "horizontalAccuracy": 0,
      "geo": {
        "type": "Point",
        "coordinates": [
          77.03485,
          43.3556916667,
          0
        ]
      }
    },
    {
      "identifier": "4c2d1dc0-51d2-41ba-86e6-fe28d29c5556",
      "name": "AAKAN0053",
      "type": "POLE",
      "elevation": 681.981,
      "verticalAccuracy": 0,
      "horizontalAccuracy": 0,
      "geo": {
        "type": "Point",
        "coordinates": [
          77.0351361111,
          43.3554222222,
          0
        ]
      }
    },
    {
      "identifier": "6e73738c-168a-45a7-a9c8-700abb224043",
      "name": "AAKAN0054",
      "type": "POLE",
      "elevation": 681.407,
      "verticalAccuracy": 0,
      "horizontalAccuracy": 0,
      "geo": {
        "type": "Point",
        "coordinates": [
          77.0360666667,
          43.3558916667,
          0
        ]
      }
    },
    {
      "identifier": "7e747881-b4d1-4f99-b614-56a3750f74f5",
      "name": "AAKAN0055",
      "type": "POLE",
      "elevation": 681.297,
      "verticalAccuracy": 0,
      "horizontalAccuracy": 0,
      "geo": {
        "type": "Point",
        "coordinates": [
          77.0372861111,
          43.3564972222,
          0
        ]
      }
    },
    {
      "identifier": "02d04f17-f5a6-4603-8701-5eb1bc0fcf51",
      "name": "AAKAN0056",
      "type": "POLE",
      "elevation": 682.799,
      "verticalAccuracy": 0,
      "horizontalAccuracy": 0,
      "geo": {
        "type": "Point",
        "coordinates": [
          77.0385111111,
          43.3571027778,
          0
        ]
      }
    },
    {
      "identifier": "470d2d0d-9dde-4a42-8212-1c4629889fbd",
      "name": "AAKAN0057",
      "type": "POLE",
      "elevation": 682.568,
      "verticalAccuracy": 0,
      "horizontalAccuracy": 0,
      "geo": {
        "type": "Point",
        "coordinates": [
          77.0397277778,
          43.3577055556,
          0
        ]
      }
    },
    {
      "identifier": "17c63297-fb29-4df7-b3e7-b95579108a02",
      "name": "AAKAN0058",
      "type": "POLE",
      "elevation": 682.962,
      "verticalAccuracy": 0,
      "horizontalAccuracy": 0,
      "geo": {
        "type": "Point",
        "coordinates": [
          77.0409416667,
          43.3583138889,
          0
        ]
      }
    },
    {
      "identifier": "059d393c-0d59-4ba4-949b-a387efd321cf",
      "name": "AAKAN0059",
      "type": "POLE",
      "elevation": 682.177,
      "verticalAccuracy": 0,
      "horizontalAccuracy": 0,
      "geo": {
        "type": "Point",
        "coordinates": [
          77.0421638889,
          43.3589194444,
          0
        ]
      }
    },
    {
      "identifier": "44958e0b-f46e-4213-92d8-07d9dd6410f8",
      "name": "AAKAN0060",
      "type": "POLE",
      "elevation": 682.715,
      "verticalAccuracy": 0,
      "horizontalAccuracy": 0,
      "geo": {
        "type": "Point",
        "coordinates": [
          77.0433861111,
          43.3595333333,
          0
        ]
      }
    },
    {
      "identifier": "bdfbcd56-7560-4380-bfb0-07c5c6a30098",
      "name": "AAKAN0061",
      "type": "POLE",
      "elevation": 682.778,
      "verticalAccuracy": 0,
      "horizontalAccuracy": 0,
      "geo": {
        "type": "Point",
        "coordinates": [
          77.0430638889,
          43.3599777778,
          0
        ]
      }
    },
    {
      "identifier": "70effd89-0be4-4422-828a-bef64005e458",
      "name": "AAKAN0062",
      "type": "POLE",
      "elevation": 683.212,
      "verticalAccuracy": 0,
      "horizontalAccuracy": 0,
      "geo": {
        "type": "Point",
        "coordinates": [
          77.0436583333,
          43.3603472222,
          0
        ]
      }
    },
    {
      "identifier": "3d7c62b4-fd1b-4998-91c6-dbd76608ded1",
      "name": "AAKAN0063",
      "type": "POLE",
      "elevation": 682.175,
      "verticalAccuracy": 0,
      "horizontalAccuracy": 0,
      "geo": {
        "type": "Point",
        "coordinates": [
          77.0449583333,
          43.3609444444,
          0
        ]
      }
    },
    {
      "identifier": "02a4c2fe-100f-429d-8267-2364d445d037",
      "name": "AAKAN0064",
      "type": "POLE",
      "elevation": 683.169,
      "verticalAccuracy": 0,
      "horizontalAccuracy": 0,
      "geo": {
        "type": "Point",
        "coordinates": [
          77.0454305556,
          43.3604305556,
          0
        ]
      }
    },
    {
      "identifier": "0b5391cd-35ea-477b-9fc0-983c20a02a32",
      "name": "AAKAN0065",
      "type": "POLE",
      "elevation": 683.314,
      "verticalAccuracy": 0,
      "horizontalAccuracy": 0,
      "geo": {
        "type": "Point",
        "coordinates": [
          77.0460833333,
          43.3607666667,
          0
        ]
      }
    },
    {
      "identifier": "f98edb3f-4527-4eca-ae6f-dbd1f5912546",
      "name": "AAKAN0066",
      "type": "POLE",
      "elevation": 683.5,
      "verticalAccuracy": 0,
      "horizontalAccuracy": 0,
      "geo": {
        "type": "Point",
        "coordinates": [
          77.0473027778,
          43.3613722222,
          0
        ]
      }
    },
    {
      "identifier": "73810364-93c5-4d5f-b274-daf8515dee6c",
      "name": "AAKAN0067",
      "type": "POLE",
      "elevation": 682.999,
      "verticalAccuracy": 0,
      "horizontalAccuracy": 0,
      "geo": {
        "type": "Point",
        "coordinates": [
          77.0484222222,
          43.3619027778,
          0
        ]
      }
    },
    {
      "identifier": "101700ba-ac7f-485d-b049-ac0c31a505c5",
      "name": "AAKAN0068",
      "type": "POLE",
      "elevation": 683.304,
      "verticalAccuracy": 0,
      "horizontalAccuracy": 0,
      "geo": {
        "type": "Point",
        "coordinates": [
          77.0496583333,
          43.3625194444,
          0
        ]
      }
    },
    {
      "identifier": "61772b3b-ec92-4b27-845c-900259b1f01f",
      "name": "AAKAN0069",
      "type": "POLE",
      "elevation": 683.334,
      "verticalAccuracy": 0,
      "horizontalAccuracy": 0,
      "geo": {
        "type": "Point",
        "coordinates": [
          77.0509583333,
          43.3631694444,
          0
        ]
      }
    },
    {
      "identifier": "6e2e795d-8368-48a6-bf17-fb4c99eb1a62",
      "name": "AAKAN0070",
      "type": "POLE",
      "elevation": 684.021,
      "verticalAccuracy": 0,
      "horizontalAccuracy": 0,
      "geo": {
        "type": "Point",
        "coordinates": [
          77.0521916667,
          43.3637722222,
          0
        ]
      }
    },
    {
      "identifier": "2d161169-a8ef-4a4d-b165-3559b63fbcec",
      "name": "AAKAN0071",
      "type": "POLE",
      "elevation": 684.241,
      "verticalAccuracy": 0,
      "horizontalAccuracy": 0,
      "geo": {
        "type": "Point",
        "coordinates": [
          77.053375,
          43.364375,
          0
        ]
      }
    },
    {
      "identifier": "28bb4254-6121-422c-bc8a-b5f893ea05d8",
      "name": "AAKAN0072",
      "type": "POLE",
      "elevation": 683.83,
      "verticalAccuracy": 0,
      "horizontalAccuracy": 0,
      "geo": {
        "type": "Point",
        "coordinates": [
          77.0546083333,
          43.3650027778,
          0
        ]
      }
    },
    {
      "identifier": "feae3670-b399-4880-926a-17588a9786ca",
      "name": "AAKAN0073",
      "type": "POLE",
      "elevation": 683.814,
      "verticalAccuracy": 0,
      "horizontalAccuracy": 0,
      "geo": {
        "type": "Point",
        "coordinates": [
          77.0558222222,
          43.3656194444,
          0
        ]
      }
    },
    {
      "identifier": "b8e76c17-6235-48da-bc24-eb47f3794b56",
      "name": "AAKAN0074",
      "type": "POLE",
      "elevation": 684.404,
      "verticalAccuracy": 0,
      "horizontalAccuracy": 0,
      "geo": {
        "type": "Point",
        "coordinates": [
          77.0570972222,
          43.3662666667,
          0
        ]
      }
    },
    {
      "identifier": "08fec8ac-6437-459c-94fb-20077353d194",
      "name": "AAKAN0075",
      "type": "POLE",
      "elevation": 684.54,
      "verticalAccuracy": 0,
      "horizontalAccuracy": 0,
      "geo": {
        "type": "Point",
        "coordinates": [
          77.058275,
          43.3668333333,
          0
        ]
      }
    },
    {
      "identifier": "9bf85590-7d23-498b-8fca-494d8283eb46",
      "name": "AAKAN0076",
      "type": "POLE",
      "elevation": 684.94,
      "verticalAccuracy": 0,
      "horizontalAccuracy": 0,
      "geo": {
        "type": "Point",
        "coordinates": [
          77.0594888889,
          43.3674527778,
          0
        ]
      }
    },
    {
      "identifier": "4fb7fb85-36d6-45f6-bdd3-c4642cc63c3c",
      "name": "AAKAN0077",
      "type": "POLE",
      "elevation": 683.989,
      "verticalAccuracy": 0,
      "horizontalAccuracy": 0,
      "geo": {
        "type": "Point",
        "coordinates": [
          77.0591944444,
          43.3675,
          0
        ]
      }
    },
    {
      "identifier": "8f0113ba-3092-4213-bde2-676d6d194e46",
      "name": "AAKAN0078",
      "type": "POLE",
      "elevation": 684.205,
      "verticalAccuracy": 0,
      "horizontalAccuracy": 0,
      "geo": {
        "type": "Point",
        "coordinates": [
          77.0598833333,
          43.3681222222,
          0
        ]
      }
    },
    {
      "identifier": "1fd11a24-2954-43d1-b787-e1cef09170ee",
      "name": "AAKAN0079",
      "type": "POLE",
      "elevation": 684.694,
      "verticalAccuracy": 0,
      "horizontalAccuracy": 0,
      "geo": {
        "type": "Point",
        "coordinates": [
          77.0602833333,
          43.3678083333,
          0
        ]
      }
    },
    {
      "identifier": "d6e8b0bb-f2d5-4234-ba62-18ec3d525dfc",
      "name": "AAKAN0080",
      "type": "POLE",
      "elevation": 684.443,
      "verticalAccuracy": 0,
      "horizontalAccuracy": 0,
      "geo": {
        "type": "Point",
        "coordinates": [
          77.0615138889,
          43.3684666667,
          0
        ]
      }
    },
    {
      "identifier": "1daccef8-91e1-4697-9f7c-7f6e30c4d328",
      "name": "AAKAN0081",
      "type": "POLE",
      "elevation": 684.256,
      "verticalAccuracy": 0,
      "horizontalAccuracy": 0,
      "geo": {
        "type": "Point",
        "coordinates": [
          77.0627861111,
          43.3691055556,
          0
        ]
      }
    },
    {
      "identifier": "9e0c330a-4ecd-4364-bb2e-8f508e0f917e",
      "name": "AAKAN0082",
      "type": "POLE",
      "elevation": 684.239,
      "verticalAccuracy": 0,
      "horizontalAccuracy": 0,
      "geo": {
        "type": "Point",
        "coordinates": [
          77.0640027778,
          43.3697194444,
          0
        ]
      }
    },
    {
      "identifier": "817099d6-b039-462f-8eb7-6a97289ea2e8",
      "name": "AAKAN0083",
      "type": "POLE",
      "elevation": 683.746,
      "verticalAccuracy": 0,
      "horizontalAccuracy": 0,
      "geo": {
        "type": "Point",
        "coordinates": [
          77.065225,
          43.3703333333,
          0
        ]
      }
    },
    {
      "identifier": "c1902a4e-2073-4b36-a8b8-4c483a9eb01f",
      "name": "AAKAN0084",
      "type": "POLE",
      "elevation": 683.802,
      "verticalAccuracy": 0,
      "horizontalAccuracy": 0,
      "geo": {
        "type": "Point",
        "coordinates": [
          77.0663805556,
          43.3709166667,
          0
        ]
      }
    },
    {
      "identifier": "2c26d54e-28c0-4f15-9229-a52004fc79e0",
      "name": "AAKAN0085",
      "type": "POLE",
      "elevation": 683.091,
      "verticalAccuracy": 0,
      "horizontalAccuracy": 0,
      "geo": {
        "type": "Point",
        "coordinates": [
          77.0676916667,
          43.3715305556,
          0
        ]
      }
    },
    {
      "identifier": "33bfae20-7581-49a6-b6a8-8695234a5da1",
      "name": "AAKAN0086",
      "type": "POLE",
      "elevation": 681.842,
      "verticalAccuracy": 0,
      "horizontalAccuracy": 0,
      "geo": {
        "type": "Point",
        "coordinates": [
          77.0689166667,
          43.3721805556,
          0
        ]
      }
    },
    {
      "identifier": "3f669585-e2e6-496d-8701-225b3787971e",
      "name": "AAKAN0087",
      "type": "POLE",
      "elevation": 682.567,
      "verticalAccuracy": 0,
      "horizontalAccuracy": 0,
      "geo": {
        "type": "Point",
        "coordinates": [
          77.0706027778,
          43.3719472222,
          0
        ]
      }
    },
    {
      "identifier": "cce60f73-862b-4bbb-a08a-486ff4be3c16",
      "name": "AAKAN0088",
      "type": "POLE",
      "elevation": 682.957,
      "verticalAccuracy": 0,
      "horizontalAccuracy": 0,
      "geo": {
        "type": "Point",
        "coordinates": [
          77.0712777778,
          43.3722583333,
          0
        ]
      }
    },
    {
      "identifier": "4a73c75b-baa7-4eed-bcb6-42fca002f3a9",
      "name": "AAKAN0089",
      "type": "POLE",
      "elevation": 681.805,
      "verticalAccuracy": 0,
      "horizontalAccuracy": 0,
      "geo": {
        "type": "Point",
        "coordinates": [
          77.0724972222,
          43.3728527778,
          0
        ]
      }
    },
    {
      "identifier": "d78a6cfb-a598-4222-8015-f4d0981aa216",
      "name": "AAKAN0090",
      "type": "POLE",
      "elevation": 682.437,
      "verticalAccuracy": 0,
      "horizontalAccuracy": 0,
      "geo": {
        "type": "Point",
        "coordinates": [
          77.0734,
          43.3727444444,
          0
        ]
      }
    },
    {
      "identifier": "549af99c-37b1-47f9-a141-c0af6ef7c225",
      "name": "AAKAN0091",
      "type": "POLE",
      "elevation": 681.72,
      "verticalAccuracy": 0,
      "horizontalAccuracy": 0,
      "geo": {
        "type": "Point",
        "coordinates": [
          77.0747111111,
          43.3733611111,
          0
        ]
      }
    },
    {
      "identifier": "f3f209e4-729d-4481-97a3-abae45b5027e",
      "name": "AAKAN0092",
      "type": "POLE",
      "elevation": 681.345,
      "verticalAccuracy": 0,
      "horizontalAccuracy": 0,
      "geo": {
        "type": "Point",
        "coordinates": [
          77.0759166667,
          43.3739805556,
          0
        ]
      }
    },
    {
      "identifier": "c430165a-c609-4b8d-9117-77a6bc2b93a1",
      "name": "AAKAN0093",
      "type": "POLE",
      "elevation": 681.259,
      "verticalAccuracy": 0,
      "horizontalAccuracy": 0,
      "geo": {
        "type": "Point",
        "coordinates": [
          77.0771277778,
          43.3745666667,
          0
        ]
      }
    },
    {
      "identifier": "b0fd1cca-bf0d-4c5f-9e3a-455d26caca38",
      "name": "AAKAN0094",
      "type": "POLE",
      "elevation": 678.656,
      "verticalAccuracy": 0,
      "horizontalAccuracy": 0,
      "geo": {
        "type": "Point",
        "coordinates": [
          77.0775861111,
          43.3740611111,
          0
        ]
      }
    },
    {
      "identifier": "936e55db-7ab3-4311-ab37-c16b1a20bca3",
      "name": "AAKAN0095",
      "type": "POLE",
      "elevation": 678.695,
      "verticalAccuracy": 0,
      "horizontalAccuracy": 0,
      "geo": {
        "type": "Point",
        "coordinates": [
          77.0784527778,
          43.3738972222,
          0
        ]
      }
    },
    {
      "identifier": "2b422e1e-0836-4167-9a75-622873570992",
      "name": "AAKAN0096",
      "type": "POLE",
      "elevation": 678.088,
      "verticalAccuracy": 0,
      "horizontalAccuracy": 0,
      "geo": {
        "type": "Point",
        "coordinates": [
          77.0796638889,
          43.3745388889,
          0
        ]
      }
    },
    {
      "identifier": "5f8c345a-4c61-4c57-bb92-510c84dc3915",
      "name": "AAKAN0097",
      "type": "POLE",
      "elevation": 678.164,
      "verticalAccuracy": 0,
      "horizontalAccuracy": 0,
      "geo": {
        "type": "Point",
        "coordinates": [
          77.0802722222,
          43.3748444444,
          0
        ]
      }
    },
    {
      "identifier": "78f86607-fd6a-401c-8054-a6379b4c5ff0",
      "name": "AAKAN0098",
      "type": "POLE",
      "elevation": 678.415,
      "verticalAccuracy": 0,
      "horizontalAccuracy": 0,
      "geo": {
        "type": "Point",
        "coordinates": [
          77.0805333333,
          43.3745416667,
          0
        ]
      }
    },
    {
      "identifier": "ca40ea8e-7d75-4467-8221-70da1de84065",
      "name": "AAKAN0099",
      "type": "POLE",
      "elevation": 679.059,
      "verticalAccuracy": 0,
      "horizontalAccuracy": 0,
      "geo": {
        "type": "Point",
        "coordinates": [
          77.0794638889,
          43.374,
          0
        ]
      }
    },
    {
      "identifier": "79d3dd0a-edf0-4499-9f8a-8705b6687b06",
      "name": "AAKAN0100",
      "type": "POLE",
      "elevation": 679.573,
      "verticalAccuracy": 0,
      "horizontalAccuracy": 0,
      "geo": {
        "type": "Point",
        "coordinates": [
          77.0782694444,
          43.3733416667,
          0
        ]
      }
    },
    {
      "identifier": "6f662f72-c8f8-443d-ba0c-4d8291aad2ed",
      "name": "AAKAN0101",
      "type": "POLE",
      "elevation": 679.977,
      "verticalAccuracy": 0,
      "horizontalAccuracy": 0,
      "geo": {
        "type": "Point",
        "coordinates": [
          77.0789361111,
          43.3724611111,
          0
        ]
      }
    },
    {
      "identifier": "70b1c90f-9df9-42a3-9528-d3153978b09b",
      "name": "AAKAN0102",
      "type": "POLE",
      "elevation": 680.468,
      "verticalAccuracy": 0,
      "horizontalAccuracy": 0,
      "geo": {
        "type": "Point",
        "coordinates": [
          77.0779555556,
          43.3719333333,
          0
        ]
      }
    },
    {
      "identifier": "47055771-4a41-479d-8ba1-15a1ec45ba06",
      "name": "AAKAN0103",
      "type": "POLE",
      "elevation": 681.086,
      "verticalAccuracy": 0,
      "horizontalAccuracy": 0,
      "geo": {
        "type": "Point",
        "coordinates": [
          77.0766805556,
          43.3712916667,
          0
        ]
      }
    },
    {
      "identifier": "8453c2ed-7f5d-4642-b914-c6221fdffe68",
      "name": "AAKAN0104",
      "type": "POLE",
      "elevation": 681.671,
      "verticalAccuracy": 0,
      "horizontalAccuracy": 0,
      "geo": {
        "type": "Point",
        "coordinates": [
          77.0754277778,
          43.3706555556,
          0
        ]
      }
    },
    {
      "identifier": "8708e997-3671-4125-aa7a-baad45394bef",
      "name": "AAKAN0105",
      "type": "POLE",
      "elevation": 682.721,
      "verticalAccuracy": 0,
      "horizontalAccuracy": 0,
      "geo": {
        "type": "Point",
        "coordinates": [
          77.074225,
          43.3699916667,
          0
        ]
      }
    },
    {
      "identifier": "24f17ba7-5d7f-4173-ae77-5324c33b19f4",
      "name": "AAKAN0106",
      "type": "POLE",
      "elevation": 685.827,
      "verticalAccuracy": 0,
      "horizontalAccuracy": 0,
      "geo": {
        "type": "Point",
        "coordinates": [
          77.0740722222,
          43.3693055556,
          0
        ]
      }
    },
    {
      "identifier": "832ff344-7231-496b-ab71-63df396280de",
      "name": "AAKAN0107",
      "type": "POLE",
      "elevation": 686.472,
      "verticalAccuracy": 0,
      "horizontalAccuracy": 0,
      "geo": {
        "type": "Point",
        "coordinates": [
          77.0737361111,
          43.3687888889,
          0
        ]
      }
    },
    {
      "identifier": "bb44ef25-4357-4e82-b8a8-42fbd71d63ec",
      "name": "AAKAN0108",
      "type": "POLE",
      "elevation": 687.22,
      "verticalAccuracy": 0,
      "horizontalAccuracy": 0,
      "geo": {
        "type": "Point",
        "coordinates": [
          77.0727972222,
          43.3678916667,
          0
        ]
      }
    },
    {
      "identifier": "2ac64a41-34ae-4701-b49d-6bcbad291351",
      "name": "AAKAN0109",
      "type": "POLE",
      "elevation": 687.254,
      "verticalAccuracy": 0,
      "horizontalAccuracy": 0,
      "geo": {
        "type": "Point",
        "coordinates": [
          77.0716472222,
          43.3671111111,
          0
        ]
      }
    },
    {
      "identifier": "13eef01c-85cd-4f9a-9724-dabeb919601f",
      "name": "AAKAN0110",
      "type": "POLE",
      "elevation": 688.048,
      "verticalAccuracy": 0,
      "horizontalAccuracy": 0,
      "geo": {
        "type": "Point",
        "coordinates": [
          77.0705666667,
          43.3663388889,
          0
        ]
      }
    },
    {
      "identifier": "1c066d4a-587f-4219-bb32-c3aad3b6695c",
      "name": "AAKAN0111",
      "type": "POLE",
      "elevation": 688.537,
      "verticalAccuracy": 0,
      "horizontalAccuracy": 0,
      "geo": {
        "type": "Point",
        "coordinates": [
          77.0696361111,
          43.3654666667,
          0
        ]
      }
    },
    {
      "identifier": "9bad480c-fc47-4784-82bb-ca9efb774ff4",
      "name": "AAKAN0112",
      "type": "POLE",
      "elevation": 689.484,
      "verticalAccuracy": 0,
      "horizontalAccuracy": 0,
      "geo": {
        "type": "Point",
        "coordinates": [
          77.0685638889,
          43.3646472222,
          0
        ]
      }
    },
    {
      "identifier": "4a0cdd36-0763-4286-afc5-75c0d0aac34d",
      "name": "AAKAN0113",
      "type": "POLE",
      "elevation": 690.137,
      "verticalAccuracy": 0,
      "horizontalAccuracy": 0,
      "geo": {
        "type": "Point",
        "coordinates": [
          77.0674194444,
          43.363775,
          0
        ]
      }
    },
    {
      "identifier": "e6108e65-52f3-41f4-b642-fe9c0ab8e6a2",
      "name": "AAKAN0114",
      "type": "POLE",
      "elevation": 690.634,
      "verticalAccuracy": 0,
      "horizontalAccuracy": 0,
      "geo": {
        "type": "Point",
        "coordinates": [
          77.0663611111,
          43.3629583333,
          0
        ]
      }
    },
    {
      "identifier": "c320e4d7-77f1-45c6-a34c-fd47bbde990b",
      "name": "AAKAN0115",
      "type": "POLE",
      "elevation": 691.689,
      "verticalAccuracy": 0,
      "horizontalAccuracy": 0,
      "geo": {
        "type": "Point",
        "coordinates": [
          77.0652666667,
          43.3622666667,
          0
        ]
      }
    },
    {
      "identifier": "e0921b48-3d5e-4f63-9b3a-5eed8b3105a0",
      "name": "AAKAN0116",
      "type": "POLE",
      "elevation": 691.125,
      "verticalAccuracy": 0,
      "horizontalAccuracy": 0,
      "geo": {
        "type": "Point",
        "coordinates": [
          77.0638694444,
          43.3616222222,
          0
        ]
      }
    },
    {
      "identifier": "947e00be-0bde-4cbc-a0b1-983c95efaa42",
      "name": "AAKAN0117",
      "type": "POLE",
      "elevation": 690.217,
      "verticalAccuracy": 0,
      "horizontalAccuracy": 0,
      "geo": {
        "type": "Point",
        "coordinates": [
          77.0626666667,
          43.361025,
          0
        ]
      }
    },
    {
      "identifier": "46a07b61-8b7f-4ab9-a0be-a38d11171458",
      "name": "AAKAN0118",
      "type": "POLE",
      "elevation": 689.981,
      "verticalAccuracy": 0,
      "horizontalAccuracy": 0,
      "geo": {
        "type": "Point",
        "coordinates": [
          77.0613916667,
          43.3604,
          0
        ]
      }
    },
    {
      "identifier": "a4917486-c046-4252-a902-f0336e506198",
      "name": "AAKAN0119",
      "type": "POLE",
      "elevation": 690.072,
      "verticalAccuracy": 0,
      "horizontalAccuracy": 0,
      "geo": {
        "type": "Point",
        "coordinates": [
          77.0601111111,
          43.359775,
          0
        ]
      }
    },
    {
      "identifier": "41fa324f-7584-4845-8eff-efe665d7cf9f",
      "name": "AAKAN0120",
      "type": "POLE",
      "elevation": 690.037,
      "verticalAccuracy": 0,
      "horizontalAccuracy": 0,
      "geo": {
        "type": "Point",
        "coordinates": [
          77.0587611111,
          43.3592555556,
          0
        ]
      }
    },
    {
      "identifier": "2d6e5914-1dcc-49e1-b4ea-f3e02677d5b9",
      "name": "AAKAN0121",
      "type": "POLE",
      "elevation": 689.204,
      "verticalAccuracy": 0,
      "horizontalAccuracy": 0,
      "geo": {
        "type": "Point",
        "coordinates": [
          77.0580138889,
          43.3593916667,
          0
        ]
      }
    },
    {
      "identifier": "e922d640-a939-42b0-86b7-94d2c0da7584",
      "name": "AAKAN0122",
      "type": "POLE",
      "elevation": 688.686,
      "verticalAccuracy": 0,
      "horizontalAccuracy": 0,
      "geo": {
        "type": "Point",
        "coordinates": [
          77.0573,
          43.3595027778,
          0
        ]
      }
    },
    {
      "identifier": "d4b303c3-6649-47e7-a672-4329b1249f58",
      "name": "AAKAN0123",
      "type": "POLE",
      "elevation": 688.348,
      "verticalAccuracy": 0,
      "horizontalAccuracy": 0,
      "geo": {
        "type": "Point",
        "coordinates": [
          77.0563527778,
          43.3590694444,
          0
        ]
      }
    },
    {
      "identifier": "8e509e2d-9a82-4394-958a-aa5b0eb5dbb5",
      "name": "AAKAN0124",
      "type": "POLE",
      "elevation": 688.243,
      "verticalAccuracy": 0,
      "horizontalAccuracy": 0,
      "geo": {
        "type": "Point",
        "coordinates": [
          77.0551666667,
          43.3585083333,
          0
        ]
      }
    },
    {
      "identifier": "6519d606-755e-46ea-8b7f-156d1b30e30e",
      "name": "AAKAN0125",
      "type": "POLE",
      "elevation": 687.766,
      "verticalAccuracy": 0,
      "horizontalAccuracy": 0,
      "geo": {
        "type": "Point",
        "coordinates": [
          77.0537972222,
          43.3578055556,
          0
        ]
      }
    },
    {
      "identifier": "52136b32-1358-4773-9615-d8f0a921fea9",
      "name": "AAKAN0126",
      "type": "POLE",
      "elevation": 687.934,
      "verticalAccuracy": 0,
      "horizontalAccuracy": 0,
      "geo": {
        "type": "Point",
        "coordinates": [
          77.052625,
          43.3569444444,
          0
        ]
      }
    },
    {
      "identifier": "45e0f1aa-607d-4dfd-912d-737037818ae2",
      "name": "AAKAN0127",
      "type": "POLE",
      "elevation": 687.926,
      "verticalAccuracy": 0,
      "horizontalAccuracy": 0,
      "geo": {
        "type": "Point",
        "coordinates": [
          77.0513527778,
          43.3565805556,
          0
        ]
      }
    },
    {
      "identifier": "9f6f6972-c4ba-463a-812b-3cd6b8105f93",
      "name": "AAKAN0128",
      "type": "POLE",
      "elevation": 688.061,
      "verticalAccuracy": 0,
      "horizontalAccuracy": 0,
      "geo": {
        "type": "Point",
        "coordinates": [
          77.0500666667,
          43.3559388889,
          0
        ]
      }
    },
    {
      "identifier": "91e16895-8c11-4f2e-b9e5-da6a63346403",
      "name": "AAKAN0129",
      "type": "POLE",
      "elevation": 688.398,
      "verticalAccuracy": 0,
      "horizontalAccuracy": 0,
      "geo": {
        "type": "Point",
        "coordinates": [
          77.0488055556,
          43.355325,
          0
        ]
      }
    },
    {
      "identifier": "bc1ac67d-cb33-4249-a6f5-45c48190013c",
      "name": "AAKAN0130",
      "type": "POLE",
      "elevation": 688.24,
      "verticalAccuracy": 0,
      "horizontalAccuracy": 0,
      "geo": {
        "type": "Point",
        "coordinates": [
          77.0475166667,
          43.3546833333,
          0
        ]
      }
    },
    {
      "identifier": "a51d39b7-7db1-44ee-8bf2-1e2dacc3d4e3",
      "name": "AAKAN0131",
      "type": "POLE",
      "elevation": 688.494,
      "verticalAccuracy": 0,
      "horizontalAccuracy": 0,
      "geo": {
        "type": "Point",
        "coordinates": [
          77.0462333333,
          43.3540527778,
          0
        ]
      }
    },
    {
      "identifier": "665a7648-05dd-4dd3-9ed1-ec7860f52099",
      "name": "AAKAN0132",
      "type": "POLE",
      "elevation": 688.721,
      "verticalAccuracy": 0,
      "horizontalAccuracy": 0,
      "geo": {
        "type": "Point",
        "coordinates": [
          77.0449555556,
          43.3534277778,
          0
        ]
      }
    },
    {
      "identifier": "11ce9091-00ba-4615-8871-3751ede8d7ac",
      "name": "AAKAN0133",
      "type": "POLE",
      "elevation": 688.061,
      "verticalAccuracy": 0,
      "horizontalAccuracy": 0,
      "geo": {
        "type": "Point",
        "coordinates": [
          77.0436722222,
          43.3527972222,
          0
        ]
      }
    },
    {
      "identifier": "92de03cd-2788-4600-90e3-da83b4ca2edc",
      "name": "AAKAN0134",
      "type": "POLE",
      "elevation": 687.197,
      "verticalAccuracy": 0,
      "horizontalAccuracy": 0,
      "geo": {
        "type": "Point",
        "coordinates": [
          77.0423972222,
          43.3521555556,
          0
        ]
      }
    },
    {
      "identifier": "44f23c6c-e256-45ca-a2f4-2fb7ee029eb2",
      "name": "AAKAN0135",
      "type": "POLE",
      "elevation": 687.11,
      "verticalAccuracy": 0,
      "horizontalAccuracy": 0,
      "geo": {
        "type": "Point",
        "coordinates": [
          77.041125,
          43.3515222222,
          0
        ]
      }
    },
    {
      "identifier": "20b0fa18-18ae-433d-bc44-11eadfa88a6b",
      "name": "AAKAN0136",
      "type": "POLE",
      "elevation": 686.435,
      "verticalAccuracy": 0,
      "horizontalAccuracy": 0,
      "geo": {
        "type": "Point",
        "coordinates": [
          77.0398111111,
          43.3509527778,
          0
        ]
      }
    },
    {
      "identifier": "edf98ec3-34d3-4637-b6ad-49a124965ec7",
      "name": "AAKAN0137",
      "type": "POLE",
      "elevation": 685.588,
      "verticalAccuracy": 0,
      "horizontalAccuracy": 0,
      "geo": {
        "type": "Point",
        "coordinates": [
          77.0385222222,
          43.3503194444,
          0
        ]
      }
    },
    {
      "identifier": "55c6712c-e78c-4763-b2c9-ccecd33f490a",
      "name": "AAKAN0138",
      "type": "POLE",
      "elevation": 685.156,
      "verticalAccuracy": 0,
      "horizontalAccuracy": 0,
      "geo": {
        "type": "Point",
        "coordinates": [
          77.0372833333,
          43.3496444444,
          0
        ]
      }
    },
    {
      "identifier": "043adf8b-ee09-4d37-a7ee-1ebc74a18f9e",
      "name": "AAKAN0139",
      "type": "POLE",
      "elevation": 684.963,
      "verticalAccuracy": 0,
      "horizontalAccuracy": 0,
      "geo": {
        "type": "Point",
        "coordinates": [
          77.035975,
          43.3490444444,
          0
        ]
      }
    },
    {
      "identifier": "a2c38583-8e58-408c-a463-e9c3d642acce",
      "name": "AAKAN0140",
      "type": "POLE",
      "elevation": 684.496,
      "verticalAccuracy": 0,
      "horizontalAccuracy": 0,
      "geo": {
        "type": "Point",
        "coordinates": [
          77.0346861111,
          43.3484194444,
          0
        ]
      }
    },
    {
      "identifier": "f9cd2547-6410-4661-b441-fbddc7fe0ff8",
      "name": "AAKAN0141",
      "type": "POLE",
      "elevation": 684.061,
      "verticalAccuracy": 0,
      "horizontalAccuracy": 0,
      "geo": {
        "type": "Point",
        "coordinates": [
          77.0334305556,
          43.3477805556,
          0
        ]
      }
    },
    {
      "identifier": "0799e720-dff5-4002-a35b-05ded3ae94fc",
      "name": "AAKAN0142",
      "type": "POLE",
      "elevation": 684.612,
      "verticalAccuracy": 0,
      "horizontalAccuracy": 0,
      "geo": {
        "type": "Point",
        "coordinates": [
          77.0321694444,
          43.3471194444,
          0
        ]
      }
    },
    {
      "identifier": "df780b7f-df3d-41e4-9959-da3b8c937faf",
      "name": "AAKAN0143",
      "type": "POLE",
      "elevation": 685.235,
      "verticalAccuracy": 0,
      "horizontalAccuracy": 0,
      "geo": {
        "type": "Point",
        "coordinates": [
          77.0308944444,
          43.346475,
          0
        ]
      }
    },
    {
      "identifier": "27794e54-933b-48cf-9397-bef431b0b7c5",
      "name": "AAKAN0144",
      "type": "POLE",
      "elevation": 685.426,
      "verticalAccuracy": 0,
      "horizontalAccuracy": 0,
      "geo": {
        "type": "Point",
        "coordinates": [
          77.0295361111,
          43.3458166667,
          0
        ]
      }
    },
    {
      "identifier": "d10312d8-3d90-46a1-b528-e8667a155e6b",
      "name": "AAKAN0145",
      "type": "POLE",
      "elevation": 685.574,
      "verticalAccuracy": 0,
      "horizontalAccuracy": 0,
      "geo": {
        "type": "Point",
        "coordinates": [
          77.0283472222,
          43.3452055556,
          0
        ]
      }
    },
    {
      "identifier": "ed0e6b2a-5bd1-45f9-bb18-d4e87c3d4c10",
      "name": "AAKAN0146",
      "type": "POLE",
      "elevation": 686.13,
      "verticalAccuracy": 0,
      "horizontalAccuracy": 0,
      "geo": {
        "type": "Point",
        "coordinates": [
          77.0270694444,
          43.3445694444,
          0
        ]
      }
    },
    {
      "identifier": "5ed852ec-bc25-489b-b107-f68210b52648",
      "name": "AAKAN0147",
      "type": "POLE",
      "elevation": 686.648,
      "verticalAccuracy": 0,
      "horizontalAccuracy": 0,
      "geo": {
        "type": "Point",
        "coordinates": [
          77.0257861111,
          43.3439277778,
          0
        ]
      }
    },
    {
      "identifier": "bd5bb382-9579-498e-9daf-483f822eb6c2",
      "name": "AAKAN0148",
      "type": "POLE",
      "elevation": 687.077,
      "verticalAccuracy": 0,
      "horizontalAccuracy": 0,
      "geo": {
        "type": "Point",
        "coordinates": [
          77.0245638889,
          43.3432472222,
          0
        ]
      }
    },
    {
      "identifier": "bc828f0e-3a7f-47f6-bab3-92be4182a7f2",
      "name": "AAKAN0149",
      "type": "POLE",
      "elevation": 686.973,
      "verticalAccuracy": 0,
      "horizontalAccuracy": 0,
      "geo": {
        "type": "Point",
        "coordinates": [
          77.023875,
          43.3422638889,
          0
        ]
      }
    },
    {
      "identifier": "fe0adfe9-bd87-45e0-834b-bf818636651b",
      "name": "AAKAN0150",
      "type": "POLE",
      "elevation": 686.695,
      "verticalAccuracy": 0,
      "horizontalAccuracy": 0,
      "geo": {
        "type": "Point",
        "coordinates": [
          77.0226416667,
          43.3415861111,
          0
        ]
      }
    },
    {
      "identifier": "a97ea0da-9d03-453c-933d-34b689cd1bb0",
      "name": "AAKAN0151",
      "type": "POLE",
      "elevation": 686.976,
      "verticalAccuracy": 0,
      "horizontalAccuracy": 0,
      "geo": {
        "type": "Point",
        "coordinates": [
          77.0214583333,
          43.3408638889,
          0
        ]
      }
    },
    {
      "identifier": "6757b30c-570f-475c-ab22-fecbd4531e91",
      "name": "AAKAN0152",
      "type": "POLE",
      "elevation": 688.076,
      "verticalAccuracy": 0,
      "horizontalAccuracy": 0,
      "geo": {
        "type": "Point",
        "coordinates": [
          77.0203861111,
          43.3399361111,
          0
        ]
      }
    },
    {
      "identifier": "d0cc8587-b7a4-46cd-89bd-61c325472e81",
      "name": "AAKAN0153",
      "type": "POLE",
      "elevation": 687.923,
      "verticalAccuracy": 0,
      "horizontalAccuracy": 0,
      "geo": {
        "type": "Point",
        "coordinates": [
          77.0198361111,
          43.3396611111,
          0
        ]
      }
    },
    {
      "identifier": "125f5db0-1b70-4166-89a4-fa641bebddbd",
      "name": "AAKAN0154",
      "type": "POLE",
      "elevation": 688.054,
      "verticalAccuracy": 0,
      "horizontalAccuracy": 0,
      "geo": {
        "type": "Point",
        "coordinates": [
          77.0200277778,
          43.3395944444,
          0
        ]
      }
    },
    {
      "identifier": "667aa06c-8121-44be-9549-19f04bcdf18b",
      "name": "AAKAN0155",
      "type": "POLE",
      "elevation": 688.064,
      "verticalAccuracy": 0,
      "horizontalAccuracy": 0,
      "geo": {
        "type": "Point",
        "coordinates": [
          77.0190805556,
          43.3389944444,
          0
        ]
      }
    },
    {
      "identifier": "18989244-8a70-4e74-9c26-d3ec990fa4c3",
      "name": "AAKAN0156",
      "type": "POLE",
      "elevation": 687.964,
      "verticalAccuracy": 0,
      "horizontalAccuracy": 0,
      "geo": {
        "type": "Point",
        "coordinates": [
          77.0183944444,
          43.3395444444,
          0
        ]
      }
    },
    {
      "identifier": "4fac464d-09a4-4297-acf0-f753e44561cf",
      "name": "AAKAN0157",
      "type": "POLE",
      "elevation": 684.341,
      "verticalAccuracy": 0,
      "horizontalAccuracy": 0,
      "geo": {
        "type": "Point",
        "coordinates": [
          77.0176861111,
          43.3398416667,
          0
        ]
      }
    },
    {
      "identifier": "b417ee87-6a3b-470e-af8b-d94dc567d5d5",
      "name": "AAKAN0158",
      "type": "POLE",
      "elevation": 684.512,
      "verticalAccuracy": 0,
      "horizontalAccuracy": 0,
      "geo": {
        "type": "Point",
        "coordinates": [
          77.0170666667,
          43.3395,
          0
        ]
      }
    },
    {
      "identifier": "ae30cdcc-745c-4f10-9f6d-bdc876560398",
      "name": "AAKAN0159",
      "type": "POLE",
      "elevation": 684.703,
      "verticalAccuracy": 0,
      "horizontalAccuracy": 0,
      "geo": {
        "type": "Point",
        "coordinates": [
          77.01575,
          43.3389055556,
          0
        ]
      }
    },
    {
      "identifier": "3866be10-c891-4dc2-80d4-3da807683a11",
      "name": "AAKAN0160",
      "type": "POLE",
      "elevation": 685.328,
      "verticalAccuracy": 0,
      "horizontalAccuracy": 0,
      "geo": {
        "type": "Point",
        "coordinates": [
          77.0144611111,
          43.3382861111,
          0
        ]
      }
    },
    {
      "identifier": "e98f534b-0d76-48bf-9049-afd44c35c71d",
      "name": "AAKAN0161",
      "type": "POLE",
      "elevation": 685.652,
      "verticalAccuracy": 0,
      "horizontalAccuracy": 0,
      "geo": {
        "type": "Point",
        "coordinates": [
          77.0132166667,
          43.3376277778,
          0
        ]
      }
    },
    {
      "identifier": "1820985e-4fb3-4b25-b6b1-ec2b77d62547",
      "name": "AAKAN0162",
      "type": "POLE",
      "elevation": 686.265,
      "verticalAccuracy": 0,
      "horizontalAccuracy": 0,
      "geo": {
        "type": "Point",
        "coordinates": [
          77.0119611111,
          43.3369694444,
          0
        ]
      }
    },
    {
      "identifier": "becc1579-5dd7-4c89-a001-cdb96a922b70",
      "name": "AAKAN0163",
      "type": "POLE",
      "elevation": 686.61,
      "verticalAccuracy": 0,
      "horizontalAccuracy": 0,
      "geo": {
        "type": "Point",
        "coordinates": [
          77.010775,
          43.3364583333,
          0
        ]
      }
    },
    {
      "identifier": "a72fdb0e-b47f-42f3-93f7-bd87346236c9",
      "name": "AAKAN0164",
      "type": "POLE",
      "elevation": 685.97,
      "verticalAccuracy": 0,
      "horizontalAccuracy": 0,
      "geo": {
        "type": "Point",
        "coordinates": [
          77.0104472222,
          43.3371361111,
          0
        ]
      }
    },
    {
      "identifier": "e1baf96a-33b8-42d7-adf6-4d5adfa2e736",
      "name": "AAKAN0165",
      "type": "POLE",
      "elevation": 685.285,
      "verticalAccuracy": 0,
      "horizontalAccuracy": 0,
      "geo": {
        "type": "Point",
        "coordinates": [
          77.0099583333,
          43.3379027778,
          0
        ]
      }
    },
    {
      "identifier": "49e32fdc-6386-43ef-a74c-0569921823b9",
      "name": "AAKAN0166",
      "type": "POLE",
      "elevation": 685.281,
      "verticalAccuracy": 0,
      "horizontalAccuracy": 0,
      "geo": {
        "type": "Point",
        "coordinates": [
          77.0102805556,
          43.3384277778,
          0
        ]
      }
    },
    {
      "identifier": "ff9e442c-7477-47c5-93ff-48458f7138ac",
      "name": "AAKAN0167",
      "type": "POLE",
      "elevation": 683.962,
      "verticalAccuracy": 0,
      "horizontalAccuracy": 0,
      "geo": {
        "type": "Point",
        "coordinates": [
          77.0109861111,
          43.3394277778,
          0
        ]
      }
    },
    {
      "identifier": "24f88552-e86f-4981-a80b-1e498a6ad41f",
      "name": "AAKAN0168",
      "type": "POLE",
      "elevation": 683.217,
      "verticalAccuracy": 0,
      "horizontalAccuracy": 0,
      "geo": {
        "type": "Point",
        "coordinates": [
          77.0116833333,
          43.3404861111,
          0
        ]
      }
    },
    {
      "identifier": "52b4ac96-80e5-46d9-9d8a-e9229df7f057",
      "name": "AAKAN0169",
      "type": "POLE",
      "elevation": 682.723,
      "verticalAccuracy": 0,
      "horizontalAccuracy": 0,
      "geo": {
        "type": "Point",
        "coordinates": [
          77.0123055556,
          43.3414694444,
          0
        ]
      }
    },
    {
      "identifier": "7caea149-3a50-43eb-bcbf-5a0ebbaa1a9c",
      "name": "AAKAN0170",
      "type": "POLE",
      "elevation": 682.42,
      "verticalAccuracy": 0,
      "horizontalAccuracy": 0,
      "geo": {
        "type": "Point",
        "coordinates": [
          77.0136194444,
          43.3420555556,
          0
        ]
      }
    },
    {
      "identifier": "5a95da98-424f-4afa-bfad-3d8481e35745",
      "name": "AAKAN0171",
      "type": "ANTENNA",
      "elevation": 702.05,
      "verticalAccuracy": 0,
      "horizontalAccuracy": 0,
      "geo": {
        "type": "Point",
        "coordinates": [
          77.0292444444,
          43.354675,
          0
        ]
      }
    },
    {
      "identifier": "69774658-7623-4bc3-967f-5449e25667ca",
      "name": "AAKAN0174",
      "type": "NAVAID",
      "elevation": 706.3,
      "verticalAccuracy": 0,
      "horizontalAccuracy": 0,
      "geo": {
        "type": "Point",
        "coordinates": [
          77.0291897778,
          43.3546554444,
          0
        ]
      }
    },
    {
      "identifier": "a2198b68-c467-4131-af13-352cb14579fa",
      "name": "AAKAN0175",
      "type": "ANTENNA",
      "elevation": 729,
      "verticalAccuracy": 0,
      "horizontalAccuracy": 0,
      "geo": {
        "type": "Point",
        "coordinates": [
          76.9776527778,
          43.3129611111,
          0
        ]
      }
    },
    {
      "identifier": "3ca1d67a-ee63-47f4-9107-f101cea848ed",
      "name": "AAKAN0177",
      "type": "ANTENNA",
      "elevation": 681.735,
      "verticalAccuracy": 0,
      "horizontalAccuracy": 0,
      "geo": {
        "type": "Point",
        "coordinates": [
          77.0228611111,
          43.3427416667,
          0
        ]
      }
    },
    {
      "identifier": "3ca1d67a-ee63-47f4-9107-f101cea848ed",
      "name": "AAKAN0177",
      "type": "ANTENNA",
      "elevation": 681.735,
      "verticalAccuracy": 0,
      "horizontalAccuracy": 0,
      "geo": {
        "type": "Point",
        "coordinates": [
          77.0231638889,
          43.3428944444,
          0
        ]
      }
    },
    {
      "identifier": "3ca1d67a-ee63-47f4-9107-f101cea848ed",
      "name": "AAKAN0177",
      "type": "ANTENNA",
      "elevation": 681.735,
      "verticalAccuracy": 0,
      "horizontalAccuracy": 0,
      "geo": {
        "type": "Point",
        "coordinates": [
          77.0409472222,
          43.3515138889,
          0
        ]
      }
    },
    {
      "identifier": "3ca1d67a-ee63-47f4-9107-f101cea848ed",
      "name": "AAKAN0177",
      "type": "ANTENNA",
      "elevation": 681.735,
      "verticalAccuracy": 0,
      "horizontalAccuracy": 0,
      "geo": {
        "type": "Point",
        "coordinates": [
          77.0412555556,
          43.3516638889,
          0
        ]
      }
    },
    {
      "identifier": "3ca1d67a-ee63-47f4-9107-f101cea848ed",
      "name": "AAKAN0177",
      "type": "ANTENNA",
      "elevation": 682.735,
      "verticalAccuracy": 0,
      "horizontalAccuracy": 0,
      "geo": {
        "type": "Point",
        "coordinates": [
          77.0589277778,
          43.3604027778,
          0
        ]
      }
    },
    {
      "identifier": "3ca1d67a-ee63-47f4-9107-f101cea848ed",
      "name": "AAKAN0177",
      "type": "ANTENNA",
      "elevation": 683.735,
      "verticalAccuracy": 0,
      "horizontalAccuracy": 0,
      "geo": {
        "type": "Point",
        "coordinates": [
          77.0592388889,
          43.3605583333,
          0
        ]
      }
    },
    {
      "identifier": "f97889ef-5c2f-4e81-abe8-510efc1de551",
      "name": "AAKAN0178",
      "type": "STACK",
      "elevation": 772,
      "verticalAccuracy": 0,
      "horizontalAccuracy": 0,
      "geo": {
        "type": "Point",
        "coordinates": [
          76.98611575,
          43.2888198056,
          0
        ]
      }
    },
    {
      "identifier": "36e1dba2-3004-47e8-901f-f509de044254",
      "name": "AALLZ05LNV01",
      "type": "NAVAID",
      "elevation": 675,
      "verticalAccuracy": 0,
      "horizontalAccuracy": 0,
      "geo": {
        "type": "Point",
        "coordinates": [
          77.0774444444444,
          43.3732222222222,
          0
        ]
      }
    },
    {
      "identifier": "6b6213eb-da3d-4ff1-8eb7-67e234aa7f6b",
      "name": "AALLZ05RNV01",
      "type": "NAVAID",
      "elevation": 680,
      "verticalAccuracy": 0,
      "horizontalAccuracy": 0,
      "geo": {
        "type": "Point",
        "coordinates": [
          77.0728055555555,
          43.3686666666667,
          0
        ]
      }
    },
    {
      "identifier": "cf1d2e07-5549-410c-9bb5-0b62014a99ec",
      "name": "AALLZ23LNV01",
      "type": "NAVAID",
      "elevation": 681,
      "verticalAccuracy": 0,
      "horizontalAccuracy": 0,
      "geo": {
        "type": "Point",
        "coordinates": [
          77.0105555555556,
          43.3378055555556,
          0
        ]
      }
    },
    {
      "identifier": "c8f2e019-96cf-457e-a6f9-adb50cafbbb3",
      "name": "AALLZ23RNV01",
      "type": "NAVAID",
      "elevation": 677,
      "verticalAccuracy": 0,
      "horizontalAccuracy": 0,
      "geo": {
        "type": "Point",
        "coordinates": [
          77.018,
          43.3437777777778,
          0
        ]
      }
    },
    {
      "identifier": "467f844b-cabd-48e1-bf44-54c2b0e6bbd9",
      "name": "AALMM05RNV01",
      "type": "NAVAID",
      "elevation": 687,
      "verticalAccuracy": 0,
      "horizontalAccuracy": 0,
      "geo": {
        "type": "Point",
        "coordinates": [
          77.0053055555556,
          43.3352222222222,
          0
        ]
      }
    },
    {
      "identifier": "54c16d3b-8cbd-4c8b-ac2f-d4ded9074d0d",
      "name": "AALMM23LNV01",
      "type": "NAVAID",
      "elevation": 677,
      "verticalAccuracy": 0,
      "horizontalAccuracy": 0,
      "geo": {
        "type": "Point",
        "coordinates": [
          77.0733333333333,
          43.3689166666667,
          0
        ]
      }
    },
    {
      "identifier": "5d1caed8-59c1-4cd4-960d-fb2e3b79232e",
      "name": "AAO00001",
      "type": "STACK",
      "elevation": 821.37,
      "verticalAccuracy": 0.101,
      "horizontalAccuracy": 0.188,
      "geo": {
        "type": "Point",
        "coordinates": [
          76.931466615,
          43.2812002940001,
          0
        ]
      }
    },
    {
      "identifier": "8855c28a-5d5f-4269-8674-b244daad4790",
      "name": "AAO00002",
      "type": "STACK",
      "elevation": 825.12,
      "verticalAccuracy": 0.101,
      "horizontalAccuracy": 0.188,
      "geo": {
        "type": "Point",
        "coordinates": [
          76.931293774,
          43.2804565150001,
          0
        ]
      }
    },
    {
      "identifier": "40fca611-af20-4eef-98bf-18f5295a876c",
      "name": "AAO00003",
      "type": "STACK",
      "elevation": 825.53,
      "verticalAccuracy": 0.101,
      "horizontalAccuracy": 0.188,
      "geo": {
        "type": "Point",
        "coordinates": [
          76.9285381450001,
          43.2781191860001,
          0
        ]
      }
    },
    {
      "identifier": "8b3c1b0f-b7dd-4cc6-a47e-01477553d2ec",
      "name": "AAO00004",
      "type": "STACK",
      "elevation": 826.22,
      "verticalAccuracy": 0.101,
      "horizontalAccuracy": 0.188,
      "geo": {
        "type": "Point",
        "coordinates": [
          76.9286595120001,
          43.2786172410001,
          0
        ]
      }
    },
    {
      "identifier": "2e69b78a-c26b-43ef-95b6-13d1dad9cb2a",
      "name": "AAO00005",
      "type": "BUILDING",
      "elevation": 821.68,
      "verticalAccuracy": 0.101,
      "horizontalAccuracy": 0.188,
      "geo": {
        "type": "Point",
        "coordinates": [
          76.9739369630001,
          43.2659670210001,
          0
        ]
      }
    },
    {
      "identifier": "2ec88365-77d9-4124-9416-87d14fa905c1",
      "name": "AAO00006",
      "type": "BUILDING",
      "elevation": 821.67,
      "verticalAccuracy": 0.101,
      "horizontalAccuracy": 0.188,
      "geo": {
        "type": "Point",
        "coordinates": [
          76.973837626,
          43.2656881450001,
          0
        ]
      }
    },
    {
      "identifier": "ecb8808e-dd09-4879-8b0e-1b8674e4de7d",
      "name": "AAO00007",
      "type": "BUILDING",
      "elevation": 821.2,
      "verticalAccuracy": 0.101,
      "horizontalAccuracy": 0.188,
      "geo": {
        "type": "Point",
        "coordinates": [
          76.9729966900001,
          43.265073994,
          0
        ]
      }
    },
    {
      "identifier": "8506beec-ef05-48a0-9a07-97305209867d",
      "name": "AAO00008",
      "type": "TOWER",
      "elevation": 832.65,
      "verticalAccuracy": 0.101,
      "horizontalAccuracy": 0.188,
      "geo": {
        "type": "Point",
        "coordinates": [
          76.9466903890001,
          43.27303892,
          0
        ]
      }
    },
    {
      "identifier": "ee1dc1b9-8ea1-4458-a49b-2560290b01c8",
      "name": "AAO00009",
      "type": "ANTENNA",
      "elevation": 822.48,
      "verticalAccuracy": 0.101,
      "horizontalAccuracy": 0.188,
      "geo": {
        "type": "Point",
        "coordinates": [
          76.9455693510001,
          43.260846718,
          0
        ]
      }
    },
    {
      "identifier": "33c3d4a0-f108-43dd-847c-4c05950b662c",
      "name": "AAO00010",
      "type": "ANTENNA",
      "elevation": 823.32,
      "verticalAccuracy": 0.101,
      "horizontalAccuracy": 0.188,
      "geo": {
        "type": "Point",
        "coordinates": [
          76.936999229,
          43.2572719950001,
          0
        ]
      }
    },
    {
      "identifier": "50666613-3cdf-4cfb-b16e-1d81b09fc723",
      "name": "AAO00011",
      "type": "TREE",
      "elevation": 825.25,
      "verticalAccuracy": 0.101,
      "horizontalAccuracy": 0.188,
      "geo": {
        "type": "Point",
        "coordinates": [
          76.93936761,
          43.2566448850001,
          0
        ]
      }
    },
    {
      "identifier": "9e3b96b3-0382-43c6-869f-b1c5977c14be",
      "name": "AAO00012",
      "type": "TREE",
      "elevation": 822.8,
      "verticalAccuracy": 0.101,
      "horizontalAccuracy": 0.188,
      "geo": {
        "type": "Point",
        "coordinates": [
          76.9548618590001,
          43.260717689,
          0
        ]
      }
    },
    {
      "identifier": "49a88e7a-0d82-4e24-be55-f8f662510f3d",
      "name": "AAO00013",
      "type": "TOWER",
      "elevation": 738.62,
      "verticalAccuracy": 0.101,
      "horizontalAccuracy": 0.188,
      "geo": {
        "type": "Point",
        "coordinates": [
          77.1211678920001,
          43.3805520780001,
          0
        ]
      }
    },
    {
      "identifier": "27056ad4-c441-4509-8245-3fdedab04a6f",
      "name": "AAO00014",
      "type": "TOWER",
      "elevation": 733.65,
      "verticalAccuracy": 0.101,
      "horizontalAccuracy": 0.188,
      "geo": {
        "type": "Point",
        "coordinates": [
          77.1203673790001,
          43.38245363,
          0
        ]
      }
    },
    {
      "identifier": "49704b40-1f0d-4cf7-85c3-962f9d737b3e",
      "name": "AAO00015",
      "type": "TREE",
      "elevation": 725.17,
      "verticalAccuracy": 0.101,
      "horizontalAccuracy": 0.188,
      "geo": {
        "type": "Point",
        "coordinates": [
          76.991707697,
          43.309756457,
          0
        ]
      }
    },
    {
      "identifier": "b9508323-83fe-4372-82f9-4db96f01aedc",
      "name": "AAO00016",
      "type": "TREE",
      "elevation": 731,
      "verticalAccuracy": 0.101,
      "horizontalAccuracy": 0.188,
      "geo": {
        "type": "Point",
        "coordinates": [
          76.9909815560001,
          43.3088280800001,
          0
        ]
      }
    },
    {
      "identifier": "c0927d45-16e7-4a48-bbaa-9c6ba88b9944",
      "name": "AAO00017",
      "type": "POLE",
      "elevation": 732.51,
      "verticalAccuracy": 0.101,
      "horizontalAccuracy": 0.188,
      "geo": {
        "type": "Point",
        "coordinates": [
          77.0120475740001,
          43.3038303310001,
          0
        ]
      }
    },
    {
      "identifier": "9772ce51-b19e-4d27-a6d9-ad0658cf6126",
      "name": "AAO00018",
      "type": "TREE",
      "elevation": 744.48,
      "verticalAccuracy": 0.101,
      "horizontalAccuracy": 0.188,
      "geo": {
        "type": "Point",
        "coordinates": [
          77.0066595000001,
          43.3018708040001,
          0
        ]
      }
    },
    {
      "identifier": "8dd6cc1c-1368-4649-8f67-fad69ba16b9a",
      "name": "AAO00019",
      "type": "TREE",
      "elevation": 721.74,
      "verticalAccuracy": 0.101,
      "horizontalAccuracy": 0.188,
      "geo": {
        "type": "Point",
        "coordinates": [
          77.1131500720001,
          43.3758215920001,
          0
        ]
      }
    },
    {
      "identifier": "21a913fe-6b9f-4cb2-99c4-bde615a2404a",
      "name": "AAO00020",
      "type": "TREE",
      "elevation": 728.82,
      "verticalAccuracy": 0.101,
      "horizontalAccuracy": 0.188,
      "geo": {
        "type": "Point",
        "coordinates": [
          77.061189304,
          43.3497401830001,
          0
        ]
      }
    },
    {
      "identifier": "83dc6e4c-8629-4a61-a6f2-b32f6b65a51a",
      "name": "AAO00021",
      "type": "STACK",
      "elevation": 721.55,
      "verticalAccuracy": 0.101,
      "horizontalAccuracy": 0.188,
      "geo": {
        "type": "Point",
        "coordinates": [
          76.9784049950001,
          43.3454300610001,
          0
        ]
      }
    },
    {
      "identifier": "4f794b54-35a0-4481-ad31-f31e1c2e50dc",
      "name": "AAO00022",
      "type": "TOWER",
      "elevation": 726.21,
      "verticalAccuracy": 0.101,
      "horizontalAccuracy": 0.188,
      "geo": {
        "type": "Point",
        "coordinates": [
          76.9729596170001,
          43.335445721,
          0
        ]
      }
    },
    {
      "identifier": "68a7c443-a35d-4a35-9520-f4c973f37031",
      "name": "AAO00023",
      "type": "TREE",
      "elevation": 723.35,
      "verticalAccuracy": 0.101,
      "horizontalAccuracy": 0.188,
      "geo": {
        "type": "Point",
        "coordinates": [
          77.041489471,
          43.3273018180001,
          0
        ]
      }
    },
    {
      "identifier": "75f6fcf8-0fb6-4bd5-a777-dcc45e6c29ed",
      "name": "AAO00024",
      "type": "TREE",
      "elevation": 723.57,
      "verticalAccuracy": 0.101,
      "horizontalAccuracy": 0.188,
      "geo": {
        "type": "Point",
        "coordinates": [
          77.0411066230001,
          43.327971129,
          0
        ]
      }
    },
    {
      "identifier": "c8d800f7-15c9-4b53-891f-b6724080b8de",
      "name": "AAO00025",
      "type": "TREE",
      "elevation": 723.45,
      "verticalAccuracy": 0.101,
      "horizontalAccuracy": 0.188,
      "geo": {
        "type": "Point",
        "coordinates": [
          77.0348553100001,
          43.3309607890001,
          0
        ]
      }
    },
    {
      "identifier": "a209574a-9e70-49d0-9fe1-cf2c185a31bf",
      "name": "AAO00026",
      "type": "TREE",
      "elevation": 721.82,
      "verticalAccuracy": 0.101,
      "horizontalAccuracy": 0.188,
      "geo": {
        "type": "Point",
        "coordinates": [
          77.0309671530001,
          43.3184890750001,
          0
        ]
      }
    },
    {
      "identifier": "3e774b68-f4ad-4c34-8f56-87c04f0ea20a",
      "name": "AAO00027",
      "type": "TREE",
      "elevation": 721.38,
      "verticalAccuracy": 0.101,
      "horizontalAccuracy": 0.188,
      "geo": {
        "type": "Point",
        "coordinates": [
          77.0311167310001,
          43.318911697,
          0
        ]
      }
    },
    {
      "identifier": "76192358-c63e-414d-b356-555dd59a6446",
      "name": "AAO00028",
      "type": "MONUMENT",
      "elevation": 729.86,
      "verticalAccuracy": 0.101,
      "horizontalAccuracy": 0.188,
      "geo": {
        "type": "Point",
        "coordinates": [
          77.0256049160001,
          43.3198608610001,
          0
        ]
      }
    },
    {
      "identifier": "90a2a4e7-3a1c-4071-909f-c7f468878f9c",
      "name": "AAO00029",
      "type": "TREE",
      "elevation": 722.04,
      "verticalAccuracy": 0.101,
      "horizontalAccuracy": 0.188,
      "geo": {
        "type": "Point",
        "coordinates": [
          77.013012657,
          43.3127869740001,
          0
        ]
      }
    },
    {
      "identifier": "29dd3e74-54a7-479c-ab4d-b7df1dc78a66",
      "name": "AAO00030",
      "type": "TREE",
      "elevation": 722.47,
      "verticalAccuracy": 0.101,
      "horizontalAccuracy": 0.188,
      "geo": {
        "type": "Point",
        "coordinates": [
          77.0266561270001,
          43.321528259,
          0
        ]
      }
    },
    {
      "identifier": "90cedf3d-e8f7-4436-a604-2aff3e477dfa",
      "name": "AAO00031",
      "type": "TREE",
      "elevation": 721.88,
      "verticalAccuracy": 0.101,
      "horizontalAccuracy": 0.188,
      "geo": {
        "type": "Point",
        "coordinates": [
          77.027297887,
          43.3214898040001,
          0
        ]
      }
    },
    {
      "identifier": "51a782b1-5e7f-4f95-9f9c-9facb7921334",
      "name": "AAO00032",
      "type": "TREE",
      "elevation": 721.58,
      "verticalAccuracy": 0.101,
      "horizontalAccuracy": 0.188,
      "geo": {
        "type": "Point",
        "coordinates": [
          76.9895762130001,
          43.3133471310001,
          0
        ]
      }
    },
    {
      "identifier": "b59e1a07-745a-43f9-b9bf-5ce018548ec5",
      "name": "AAO00033",
      "type": "TREE",
      "elevation": 725.26,
      "verticalAccuracy": 0.101,
      "horizontalAccuracy": 0.188,
      "geo": {
        "type": "Point",
        "coordinates": [
          77.0266042550001,
          43.3197342070001,
          0
        ]
      }
    },
    {
      "identifier": "126c32b0-bf4e-4068-a7e8-a6170d0faf58",
      "name": "AAO00034",
      "type": "TREE",
      "elevation": 699.72,
      "verticalAccuracy": 0.101,
      "horizontalAccuracy": 0.188,
      "geo": {
        "type": "Point",
        "coordinates": [
          77.0116728830001,
          43.343084634,
          0
        ]
      }
    },
    {
      "identifier": "a1a487ab-0196-429a-9dca-0e2c8c435189",
      "name": "AAO00035",
      "type": "TREE",
      "elevation": 694.98,
      "verticalAccuracy": 0.101,
      "horizontalAccuracy": 0.188,
      "geo": {
        "type": "Point",
        "coordinates": [
          77.0819308260001,
          43.37346562,
          0
        ]
      }
    },
    {
      "identifier": "ef1cf720-ed41-49eb-98b6-a5ef4250990a",
      "name": "AAO00036",
      "type": "SIGN",
      "elevation": 680.28,
      "verticalAccuracy": 0.101,
      "horizontalAccuracy": 0.188,
      "geo": {
        "type": "Point",
        "coordinates": [
          77.061197733,
          43.3645248510001,
          0
        ]
      }
    },
    {
      "identifier": "9f474c4c-1006-4694-9060-17ccfce488e1",
      "name": "AAO00037",
      "type": "POLE",
      "elevation": 696.83,
      "verticalAccuracy": 0.101,
      "horizontalAccuracy": 0.188,
      "geo": {
        "type": "Point",
        "coordinates": [
          77.0130750210001,
          43.3431402130001,
          0
        ]
      }
    },
    {
      "identifier": "445f04e2-3beb-47a0-977e-2445b80f9b38",
      "name": "AAO00038",
      "type": "TREE",
      "elevation": 697.26,
      "verticalAccuracy": 0.101,
      "horizontalAccuracy": 0.188,
      "geo": {
        "type": "Point",
        "coordinates": [
          77.0114462990001,
          43.3421777090001,
          0
        ]
      }
    },
    {
      "identifier": "a86990cd-9046-4863-836d-0afb3d98c87c",
      "name": "AAO00039",
      "type": "POLE",
      "elevation": 688.21,
      "verticalAccuracy": 0.101,
      "horizontalAccuracy": 0.188,
      "geo": {
        "type": "Point",
        "coordinates": [
          77.073120316,
          43.3691238430001,
          0
        ]
      }
    },
    {
      "identifier": "6ded6e70-edb7-41e3-9fad-81d56635e666",
      "name": "AAO00040",
      "type": "TREE",
      "elevation": 705.87,
      "verticalAccuracy": 0.101,
      "horizontalAccuracy": 0.188,
      "geo": {
        "type": "Point",
        "coordinates": [
          77.0065321450001,
          43.333140486,
          0
        ]
      }
    },
    {
      "identifier": "ec87faed-b80b-4621-a49a-d322bf68e355",
      "name": "AAO00041",
      "type": "TREE",
      "elevation": 702.45,
      "verticalAccuracy": 0.101,
      "horizontalAccuracy": 0.188,
      "geo": {
        "type": "Point",
        "coordinates": [
          77.0068700880001,
          43.3328551910001,
          0
        ]
      }
    },
    {
      "identifier": "3cf70a80-5392-4878-ac25-7e5f14632136",
      "name": "AAO00042",
      "type": "TREE",
      "elevation": 692.14,
      "verticalAccuracy": 0.101,
      "horizontalAccuracy": 0.188,
      "geo": {
        "type": "Point",
        "coordinates": [
          77.0109254670001,
          43.3401967520001,
          0
        ]
      }
    },
    {
      "identifier": "0b5b26f4-fbf7-4522-9f3a-3c2c5bf0fbbe",
      "name": "AAO00043",
      "type": "TREE",
      "elevation": 689.48,
      "verticalAccuracy": 0.101,
      "horizontalAccuracy": 0.188,
      "geo": {
        "type": "Point",
        "coordinates": [
          77.0108435130001,
          43.3400690700001,
          0
        ]
      }
    },
    {
      "identifier": "f23902fd-4f95-4311-bcf7-96415f87d653",
      "name": "AAO00044",
      "type": "TREE",
      "elevation": 689.31,
      "verticalAccuracy": 0.101,
      "horizontalAccuracy": 0.188,
      "geo": {
        "type": "Point",
        "coordinates": [
          77.0105353010001,
          43.340106651,
          0
        ]
      }
    },
    {
      "identifier": "4be39237-e6a3-44ee-bdd6-4a193487fb96",
      "name": "AAO00045",
      "type": "TREE",
      "elevation": 695.83,
      "verticalAccuracy": 0.101,
      "horizontalAccuracy": 0.188,
      "geo": {
        "type": "Point",
        "coordinates": [
          77.0088924240001,
          43.338042332,
          0
        ]
      }
    },
    {
      "identifier": "da687c16-90bf-4b32-a7f4-4486bdab7f48",
      "name": "AAO00046",
      "type": "TREE",
      "elevation": 688.23,
      "verticalAccuracy": 0.101,
      "horizontalAccuracy": 0.188,
      "geo": {
        "type": "Point",
        "coordinates": [
          77.0141672,
          43.3377885660001,
          0
        ]
      }
    },
    {
      "identifier": "c42eb6c4-7f85-4b0e-90de-789a317722d8",
      "name": "AAO00047",
      "type": "TREE",
      "elevation": 685.43,
      "verticalAccuracy": 0.101,
      "horizontalAccuracy": 0.188,
      "geo": {
        "type": "Point",
        "coordinates": [
          77.0151736880001,
          43.3382841090001,
          0
        ]
      }
    },
    {
      "identifier": "5332d6c0-00fe-4692-8698-07679135437e",
      "name": "AAO00048",
      "type": "BUILDING",
      "elevation": 681.64,
      "verticalAccuracy": 0.101,
      "horizontalAccuracy": 0.188,
      "geo": {
        "type": "Point",
        "coordinates": [
          77.0173819230001,
          43.339697743,
          0
        ]
      }
    },
    {
      "identifier": "4ccb8cfc-115d-4e8e-b78e-caeb36e44c2c",
      "name": "AAO00049",
      "type": "POLE",
      "elevation": 683.29,
      "verticalAccuracy": 0.101,
      "horizontalAccuracy": 0.188,
      "geo": {
        "type": "Point",
        "coordinates": [
          77.066213438,
          43.365993959,
          0
        ]
      }
    },
    {
      "identifier": "1aeb7c9f-c14d-406a-961a-d7b7353a942c",
      "name": "AAO00050",
      "type": "TREE",
      "elevation": 697.43,
      "verticalAccuracy": 0.101,
      "horizontalAccuracy": 0.188,
      "geo": {
        "type": "Point",
        "coordinates": [
          77.0163819270001,
          43.3380063580001,
          0
        ]
      }
    },
    {
      "identifier": "b85af64b-ef53-47ec-afda-c1f5a7846667",
      "name": "AAO00051",
      "type": "TREE",
      "elevation": 683.85,
      "verticalAccuracy": 0.101,
      "horizontalAccuracy": 0.188,
      "geo": {
        "type": "Point",
        "coordinates": [
          77.0240422640001,
          43.3425798160001,
          0
        ]
      }
    },
    {
      "identifier": "93cca4a1-2b9f-4949-a2c2-b20122e3d502",
      "name": "AAO00052",
      "type": "TREE",
      "elevation": 687.67,
      "verticalAccuracy": 0.101,
      "horizontalAccuracy": 0.188,
      "geo": {
        "type": "Point",
        "coordinates": [
          77.0241386160001,
          43.3427448290001,
          0
        ]
      }
    },
    {
      "identifier": "7e85d89d-69e1-42c9-a38b-31d05d2dec80",
      "name": "AAO00053",
      "type": "TREE",
      "elevation": 684.42,
      "verticalAccuracy": 0.101,
      "horizontalAccuracy": 0.188,
      "geo": {
        "type": "Point",
        "coordinates": [
          77.0242955940001,
          43.342704541,
          0
        ]
      }
    },
    {
      "identifier": "3af8e888-c318-4016-8213-d0f8927d19ca",
      "name": "AAO00054",
      "type": "SIGN",
      "elevation": 681.13,
      "verticalAccuracy": 0.101,
      "horizontalAccuracy": 0.188,
      "geo": {
        "type": "Point",
        "coordinates": [
          77.0242171240001,
          43.3428416580001,
          0
        ]
      }
    },
    {
      "identifier": "81390ed1-f5a3-4e22-b86d-4e216e8fc6e1",
      "name": "AAO00055",
      "type": "BUILDING",
      "elevation": 684.24,
      "verticalAccuracy": 0.101,
      "horizontalAccuracy": 0.188,
      "geo": {
        "type": "Point",
        "coordinates": [
          77.0223905490001,
          43.3419514940001,
          0
        ]
      }
    },
    {
      "identifier": "d144ef10-a4b2-41c2-9006-07abff54b278",
      "name": "AAO00056",
      "type": "TREE",
      "elevation": 685.2,
      "verticalAccuracy": 0.101,
      "horizontalAccuracy": 0.188,
      "geo": {
        "type": "Point",
        "coordinates": [
          77.0335993940001,
          43.347065188,
          0
        ]
      }
    },
    {
      "identifier": "a7a4ee7f-4fb2-419a-9330-07dd855785ea",
      "name": "AAO00057",
      "type": "TREE",
      "elevation": 680.07,
      "verticalAccuracy": 0.101,
      "horizontalAccuracy": 0.188,
      "geo": {
        "type": "Point",
        "coordinates": [
          77.033629234,
          43.3475423740001,
          0
        ]
      }
    },
    {
      "identifier": "e1eeee6b-c51b-4444-a5e9-440888a0b1a2",
      "name": "AAO00058",
      "type": "TREE",
      "elevation": 682.79,
      "verticalAccuracy": 0.101,
      "horizontalAccuracy": 0.188,
      "geo": {
        "type": "Point",
        "coordinates": [
          77.0406706790001,
          43.3508800640001,
          0
        ]
      }
    },
    {
      "identifier": "ba539016-ec40-49dd-9c7b-4502ceb238af",
      "name": "AAO00059",
      "type": "TREE",
      "elevation": 685.06,
      "verticalAccuracy": 0.101,
      "horizontalAccuracy": 0.188,
      "geo": {
        "type": "Point",
        "coordinates": [
          77.046378888,
          43.3537274910001,
          0
        ]
      }
    },
    {
      "identifier": "9a3a3b81-496c-4246-aa7a-356d40dc9232",
      "name": "AAO00060",
      "type": "TREE",
      "elevation": 690.04,
      "verticalAccuracy": 0.101,
      "horizontalAccuracy": 0.188,
      "geo": {
        "type": "Point",
        "coordinates": [
          77.0497972960001,
          43.3550524410001,
          0
        ]
      }
    },
    {
      "identifier": "c40c9830-c005-4a85-9bd6-f93bedce52e1",
      "name": "AAO00061",
      "type": "TREE",
      "elevation": 686.44,
      "verticalAccuracy": 0.101,
      "horizontalAccuracy": 0.188,
      "geo": {
        "type": "Point",
        "coordinates": [
          77.050016289,
          43.3554004540001,
          0
        ]
      }
    },
    {
      "identifier": "d08e6ea9-919e-4b79-b031-8d88929b533b",
      "name": "AAO00062",
      "type": "TREE",
      "elevation": 698.78,
      "verticalAccuracy": 0.101,
      "horizontalAccuracy": 0.188,
      "geo": {
        "type": "Point",
        "coordinates": [
          77.0509906520001,
          43.354998015,
          0
        ]
      }
    },
    {
      "identifier": "ebeb6ffa-56e2-4bd3-9802-15b65ecca6f7",
      "name": "AAO00063",
      "type": "TREE",
      "elevation": 691.89,
      "verticalAccuracy": 0.101,
      "horizontalAccuracy": 0.188,
      "geo": {
        "type": "Point",
        "coordinates": [
          77.0504635410001,
          43.3551706090001,
          0
        ]
      }
    },
    {
      "identifier": "421b63b3-4d7c-4a42-a903-d1f08116355f",
      "name": "AAO00064",
      "type": "TREE",
      "elevation": 691.51,
      "verticalAccuracy": 0.101,
      "horizontalAccuracy": 0.188,
      "geo": {
        "type": "Point",
        "coordinates": [
          77.0502544300001,
          43.355044148,
          0
        ]
      }
    },
    {
      "identifier": "1f23691d-2579-4b69-b6eb-b5a9988c5857",
      "name": "AAO00065",
      "type": "TREE",
      "elevation": 687.86,
      "verticalAccuracy": 0.101,
      "horizontalAccuracy": 0.188,
      "geo": {
        "type": "Point",
        "coordinates": [
          77.050265891,
          43.355338277,
          0
        ]
      }
    },
    {
      "identifier": "28ab7763-1086-4b8c-afa7-e72650385ffd",
      "name": "AAO00066",
      "type": "BUILDING",
      "elevation": 684.72,
      "verticalAccuracy": 0.101,
      "horizontalAccuracy": 0.188,
      "geo": {
        "type": "Point",
        "coordinates": [
          77.049825351,
          43.3553442350001,
          0
        ]
      }
    },
    {
      "identifier": "c17ca68e-5591-4c8a-a008-1965cb2f919e",
      "name": "AAO00067",
      "type": "TREE",
      "elevation": 685.49,
      "verticalAccuracy": 0.101,
      "horizontalAccuracy": 0.188,
      "geo": {
        "type": "Point",
        "coordinates": [
          77.0526979140001,
          43.356910676,
          0
        ]
      }
    },
    {
      "identifier": "51222550-2748-4e1f-8c13-2a7627ae7c8b",
      "name": "AAO00068",
      "type": "TREE",
      "elevation": 687.48,
      "verticalAccuracy": 0.101,
      "horizontalAccuracy": 0.188,
      "geo": {
        "type": "Point",
        "coordinates": [
          77.0581405200001,
          43.3595404650001,
          0
        ]
      }
    },
    {
      "identifier": "31ea67f9-dd1c-47bb-8898-f09cc489324a",
      "name": "AAO00069",
      "type": "BUILDING",
      "elevation": 684.87,
      "verticalAccuracy": 0.101,
      "horizontalAccuracy": 0.188,
      "geo": {
        "type": "Point",
        "coordinates": [
          77.058900312,
          43.3598554640001,
          0
        ]
      }
    },
    {
      "identifier": "9e88d106-8e65-4c96-8673-56e880da8798",
      "name": "AAO00070",
      "type": "BUILDING",
      "elevation": 687.74,
      "verticalAccuracy": 0.101,
      "horizontalAccuracy": 0.188,
      "geo": {
        "type": "Point",
        "coordinates": [
          77.0595131440001,
          43.3602648090001,
          0
        ]
      }
    },
    {
      "identifier": "85277e00-253d-4670-a9d5-5091be16cb8a",
      "name": "AAO00071",
      "type": "TREE",
      "elevation": 697.24,
      "verticalAccuracy": 0.101,
      "horizontalAccuracy": 0.188,
      "geo": {
        "type": "Point",
        "coordinates": [
          77.0118369140001,
          43.335660724,
          0
        ]
      }
    },
    {
      "identifier": "fca8f58a-19e1-4e7f-902c-446be4cd4fe1",
      "name": "AAO00072",
      "type": "TREE",
      "elevation": 694.32,
      "verticalAccuracy": 0.101,
      "horizontalAccuracy": 0.188,
      "geo": {
        "type": "Point",
        "coordinates": [
          77.008086629,
          43.339403539,
          0
        ]
      }
    },
    {
      "identifier": "c6d20db7-0fc1-46f3-919f-f6f7bc1c1ac7",
      "name": "AAO00073",
      "type": "TREE",
      "elevation": 698.48,
      "verticalAccuracy": 0.101,
      "horizontalAccuracy": 0.188,
      "geo": {
        "type": "Point",
        "coordinates": [
          77.009712418,
          43.339958428,
          0
        ]
      }
    },
    {
      "identifier": "260d4441-bd51-4418-846d-3c708f9f2fb2",
      "name": "AAO00074",
      "type": "TREE",
      "elevation": 698.29,
      "verticalAccuracy": 0.101,
      "horizontalAccuracy": 0.188,
      "geo": {
        "type": "Point",
        "coordinates": [
          77.009911909,
          43.340095078,
          0
        ]
      }
    },
    {
      "identifier": "be840c16-f53d-4496-8764-29aa12094cec",
      "name": "AAO00075",
      "type": "TREE",
      "elevation": 694.27,
      "verticalAccuracy": 0.101,
      "horizontalAccuracy": 0.188,
      "geo": {
        "type": "Point",
        "coordinates": [
          77.010037611,
          43.3402473740001,
          0
        ]
      }
    },
    {
      "identifier": "9cf3edd4-851c-425e-a838-5cba2dc3cf0a",
      "name": "AAO00076",
      "type": "TREE",
      "elevation": 694.16,
      "verticalAccuracy": 0.101,
      "horizontalAccuracy": 0.188,
      "geo": {
        "type": "Point",
        "coordinates": [
          77.0102561360001,
          43.3404975020001,
          0
        ]
      }
    },
    {
      "identifier": "0920c234-e940-431d-8e86-0096884bab53",
      "name": "AAO00077",
      "type": "TREE",
      "elevation": 688.71,
      "verticalAccuracy": 0.101,
      "horizontalAccuracy": 0.188,
      "geo": {
        "type": "Point",
        "coordinates": [
          77.0111283890001,
          43.3404866760001,
          0
        ]
      }
    }
  ]
};