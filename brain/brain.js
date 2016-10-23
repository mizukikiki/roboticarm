"use strict";
var bleno = require('bleno');
var i2cBus = require('i2c-bus');
var util = require('util');

var Pca9685Driver = require("pca9685").Pca9685Driver;

// default positions
var defaultHip = 50;
var defaultShoulder = 50;
var defaultElbow = 50;
var defaultClaw = 50;

var step = 15;

var redArm = {
  "name": "red",
  "position": { "hip": defaultHip, "shoulder": defaultShoulder, 
		"elbow": defaultElbow, "claw": defaultClaw },
  "hip": { "channel": 3, "min": 1100, "max": 2300 },
  "shoulder": { "channel": 1, "min": 700, "max": 1250 },
  "elbow": { "channel": 2, "min": 1300, "max": 2200 },
  "claw": { "channel": 0, "min": 900, "max": 2300 }
}

var blueArm = {
  "name": "blue",
  "position": { "hip": defaultHip, "shoulder": defaultShoulder, 
		"elbow": defaultElbow, "claw": defaultClaw },
  "hip": { "channel": 12, "min": 1000, "max": 2400 },
  "shoulder": { "channel": 13, "min": 800, "max": 1350 },
  "elbow": { "channel": 14, "min": 1000, "max": 1900 },
  "claw": { "channel": 15, "min": 700, "max": 2100 }
}

function getPulseLength(position, min, max) {
    var delta = max - min;
    var ratio = delta / 100;
    return min + ratio * position;
}

function positionBodyPart(name, part, position, min, max, channel) {
    var pulseLength = getPulseLength(position, min, max);
    console.log(name + ' ' + part + ' set to ' + pulseLength);
    pwm.setPulseLength(channel, pulseLength);
}

function positionArm(arm) {
    positionBodyPart(arm.name, "hip", arm.position.hip, arm.hip.min,
		     arm.hip.max, arm.hip.channel);
    positionBodyPart(arm.name, "shoulder", arm.position.shoulder,
		     arm.shoulder.min, arm.shoulder.max, arm.shoulder.channel);
    positionBodyPart(arm.name, "elbow", arm.position.elbow, arm.elbow.min,
		     arm.elbow.max, arm.elbow.channel);
    positionBodyPart(arm.name, "claw", arm.position.claw, arm.claw.min,
		     arm.claw.max, arm.claw.channel);
}

function resetArm(arm) {
    arm.position.hip = defaultHip;
    arm.position.shoulder = defaultShoulder;
    arm.position.elbow = defaultElbow;
    arm.position.claw = defaultClaw; 
}

function alterPulseLength(arm, bodyPart, delta) {
    var position = arm.position[bodyPart];
    var min = arm[bodyPart].min;
    var max = arm[bodyPart].max;
    console.log(bodyPart + ' MIN ' + min + ' MAX ' + max);
    var newPosition = position + delta;
    if (newPosition < 0) {
        newPosition = 0;
    }
    else if (newPosition > 100) {
        newPosition = 100;
    }
    console.log(arm.name + ' current position ' + position + ' new position ' + newPosition); 
    arm.position[bodyPart] = newPosition;
}

function moveArm(arm, operation) {
    switch(operation) {
        case '1':
                alterPulseLength(arm, 'hip', step); 
                break;
	case '2':
                alterPulseLength(arm, 'hip', step * -1);
                break;
	case '3':
                alterPulseLength(arm, 'shoulder', step);
                break;
	case '4':
                alterPulseLength(arm, 'shoulder', step * -1);
                break;
	case '5':
                alterPulseLength(arm, 'elbow', step);
                break;
	case '6':
                alterPulseLength(arm, 'elbow', step * -1);
                break;
	case '7':
                alterPulseLength(arm, 'claw', step);
                break;
	case '8':
                alterPulseLength(arm, 'claw', step * -1);
                break;
    } 
} 

function processArms(command) {
    console.log('command ' + command);
    if (command === 'ff') {
        pwm.allChannelsOff();
    }
    else if (command === 'cc') {
        resetArm(redArm);
        resetArm(blueArm);
        positionArm(redArm);
        positionArm(blueArm);
    }
    else if (command === 'aa') {
        resetArm(redArm);
	positionArm(redArm);
    }
    else if (command === 'bb') {
        resetArm(blueArm);
        positionArm(blueArm);
    }
    else if (command.match(/[a-c][1-8]/)) {
        var regex = /(.)(\d)/
        var match = regex.exec(command);
        var armRule = match[1];
        var operation = match[2];
        if (armRule === 'a') {
            moveArm(redArm, operation);
            positionArm(redArm);
        }
	else if (armRule === 'b') {
            moveArm(blueArm, operation);
            positionArm(blueArm);
        }
        else if (armRule === 'c') {
            moveArm(redArm, operation);
            moveArm(blueArm, operation);
            positionArm(redArm);
            positionArm(blueArm);
        }
    }
}

// PCA9685 options
var options = {
    i2c: i2cBus.openSync(1),
    address: 0x40,
    frequency: 50,
    debug: true
};

var pwm = new Pca9685Driver(options, function startLoop(err) {
    if (err) {
        console.error("Error initializing PCA9685");
        process.exit(-1);
    }
});

var BlenoPrimaryService = bleno.PrimaryService;
var BlenoCharacteristic = bleno.Characteristic;

var EchoCharacteristic = function() {
  EchoCharacteristic.super_.call(this, {
    uuid: 'ec0e',
    properties: ['read', 'write', 'notify'],
    value: null
  });

  this._value = new Buffer(0);
  this._updateValueCallback = null;
};

util.inherits(EchoCharacteristic, BlenoCharacteristic);

EchoCharacteristic.prototype.onReadRequest = function(offset, callback) {
  console.log('EchoCharacteristic - onReadRequest: value = ' + this._value.toString('hex'));

  callback(this.RESULT_SUCCESS, this._value);
};

EchoCharacteristic.prototype.onWriteRequest = function(data, offset, withoutResponse, callback) {
  this._value = data;

  console.log('EchoCharacteristic - onWriteRequest: value = ' + this._value.toString('hex'));

  if (this._updateValueCallback) {
    console.log('EchoCharacteristic - onWriteRequest: notifying');

    this._updateValueCallback(this._value);
  }
  processArms(data.toString('hex'));
  callback(this.RESULT_SUCCESS);
};

EchoCharacteristic.prototype.onSubscribe = function(maxValueSize, updateValueCallback) {
  console.log('EchoCharacteristic - onSubscribe');

  this._updateValueCallback = updateValueCallback;
};

EchoCharacteristic.prototype.onUnsubscribe = function() {
  console.log('EchoCharacteristic - onUnsubscribe');

  this._updateValueCallback = null;
};

console.log('bleno - echo');

bleno.on('stateChange', function(state) {
  console.log('on -> stateChange: ' + state);


  if (state === 'poweredOn') {
    bleno.startAdvertising('echo', ['ec00']);
  } else {
    bleno.stopAdvertising();
  }
});

bleno.on('advertisingStart', function(error) {
  console.log('on -> advertisingStart: ' + (error ? 'error ' + error : 'success'));

  if (!error) {
    bleno.setServices([
      new BlenoPrimaryService({
        uuid: 'ec00',
        characteristics: [
          new EchoCharacteristic()
        ]
      })
    ]);
  }
});

// set-up CTRL-C with graceful shutdown
process.on("SIGINT", function () {
    console.log("\nGracefully shutting down from SIGINT (Ctrl-C)");
    pwm.allChannelsOff();
    setTimeout(function() { process.exit(0); }, 700);
});
