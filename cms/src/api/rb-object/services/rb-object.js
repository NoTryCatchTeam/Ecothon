'use strict';

/**
 * rb-object service
 */

const { createCoreService } = require('@strapi/strapi').factories;

module.exports = createCoreService('api::rb-object.rb-object');
