#pragma once

#include <stdint.h>

typedef union _SET_GET_EFFECT_STRUCTURE {
	struct {
		uint8_t report_id;
		uint8_t effect_type;
		uint8_t byte_count; // valid only for custom force data effect.
		     // custom force effects are not supported by this device.
	} SET_REPORT_REQUEST;
	struct {
		uint8_t report_id; // 2
		uint8_t effect_block_index; // index dell'effetto
		uint8_t block_load_status; // 1 ok, 2 -out of memory, 3 undefined.
		int ram_pool_available;
	} PID_BLOCK_LOAD_REPORT;

	uint8_t val[8];
} SET_GET_EFFECT_STRUCTURE;

typedef union _EFFECT_BLOCK {
	struct {
		uint8_t set_effect_report;
		uint8_t effect_block_index;
		uint8_t parameter_block_offset;
         
		uint8_t rom_flag;
	} effect_block_parameters;

	uint8_t val[5];
} EFFECT_BLOCK;

typedef union _EFFECT {
	struct {
		uint8_t effect_type;
		uint8_t effect_duration;
		uint8_t effect_sample_period;
		uint8_t effect_gain;
		uint8_t effect_trigger_button;
		uint8_t trigger_repeat_interval;
		uint8_t axes_enable;
		uint8_t direction_enable;
		uint8_t direction;
		uint8_t start_delay;

		uint8_t type_specific_block_handle_number;
		uint8_t type_specific_block_handle_1;// index to the type specific block1.
		uint8_t type_specific_block_handle_2;// index to the type specific block2.


	} effect_parameters;
	uint8_t val[13];
} EFFECT; // 12 effects

typedef union _ENVELOPE_BLOCK {
	struct {
		uint8_t attack_level;
		uint8_t attack_time;
		uint8_t fade_level;
		uint8_t fade_time;
	}envelope_block_parameters;

	uint8_t val[4];
} ENVELOPE_BLOCK; // 12 effects, max 7 envelopes. 1 envelope per effect that support envelopes


typedef union _CONDITION_BLOCK {
	struct {
		uint8_t cp_offset;
		uint8_t positive_coefficient;
		uint8_t negative_coefficient;
		uint8_t positive_saturation;
		uint8_t negative_saturation;
		uint8_t dead_band;
	} condition_block_parameters;

	uint8_t val[6];
} CONDITION_BLOCK; // 12 effects, max 8 conditions. 4 per axis x/y. 2 conditions(x/y) per effect that support conditions

typedef union _PERIODIC_BLOCK {
	struct {
		uint8_t offset;
		uint8_t magnitude;
		uint8_t phase;
		uint8_t period;
	}periodic_block_parameters;

	uint8_t val[4];
}PERIODIC_BLOCK;// 12 effects, max 5 PERIODIC_BLOCKs. 1 PERIODIC_BLOCK per effect that support PERIODIC_BLOCK

typedef union _CONSTANT_FORCE_BLOCK {
	struct {
		uint8_t magnitude;
	} constant_force_block_parameters;

	uint8_t val[1];
} CONSTANT_FORCE_BLOCK; // only for constant force. idk, maybe change later.
 

typedef union _EFFECT_OPERATIONS {
	struct {
		uint8_t op_effect_start;
		uint8_t op_effect_start_solo;
		uint8_t op_effect_stop;
		uint8_t loop_count;
	} effect_operations_parameters;

	uint8_t val[4];
} EFFECT_OPERATIONS;

typedef union _DEVICE_GAIN {
	struct {
		uint8_t device_gain;
	} device_gain_parameters;

	uint8_t val[1];
} DEVICE_GAIN;
