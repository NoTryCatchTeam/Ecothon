'use strict';

/**
 * rb-object router
 */

const { createCoreRouter } = require('@strapi/strapi').factories;

module.exports = createCoreRouter('api::rb-object.rb-object');
